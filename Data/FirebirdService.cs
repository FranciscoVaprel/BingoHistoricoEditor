using BingoHistoricoEditor.Models;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace BingoHistoricoEditor.Data {
    public class FirebirdService {
        private readonly string _cs;

        public FirebirdService(IConfiguration config) {
            _cs = BuildConnectionString(config);

        }

        // ── Resolución de connection string (igual que en el servicio) ────────

        private static string BuildConnectionString(IConfiguration config) {
            var s    = config.GetSection("BingoSettings");
            string modo   = s["DbModo"] ?? "Local";
            string dbPath = ResolverRuta(modo, s);

            return $"User={s["DbUser"] ?? "SYSDBA"};"
                 + $"Password={s["DbPass"] ?? "masterkey"};"
                 + $"Database={dbPath};"
                 + $"DataSource={s["DbSource"] ?? "localhost"};"
                 + $"Port={s["DbPort"] ?? "3050"};"
                 + $"Dialect={s["DbDialect"] ?? "3"};"
                 + $"Charset={s["DbCharset"] ?? "ISO8859_1"};";
        }

        private static string ResolverRuta(string modo, IConfigurationSection s) {
            if (modo.Equals("Remoto", StringComparison.OrdinalIgnoreCase) ||
                modo.Equals("Alias",  StringComparison.OrdinalIgnoreCase))
                return s["DbRemotePath"] ?? "";

            if (!string.IsNullOrWhiteSpace(s["DbAbsolutePath"]))
                return s["DbAbsolutePath"]!;

            string rel = (s["DbRelativePath"] ?? "BBDD/AAMSLOG.FDB")
                         .Replace("/",  Path.DirectorySeparatorChar.ToString())
                         .Replace("\\", Path.DirectorySeparatorChar.ToString());
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rel);
        }

        // ── Verificar conexión ────────────────────────────────────────────────

        /// <summary>
        /// Verifica si la BD existe y la ruta está configurada correctamente
        /// </summary>
        public string ObtenerRutaBD() {
            var s = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var section = s.GetSection("BingoSettings");
            string modo = section["DbModo"] ?? "Local";
            return ResolverRuta(modo, section);
        }

        /// <summary>
        /// Verifica si el archivo .fdb existe
        /// </summary>
        public bool ExisteBD() {
            string rutaBD = ObtenerRutaBD();
            return File.Exists(rutaBD);
        }

        public async Task<bool> TestConnectionAsync() {
            try {
                using var conn = new FbConnection(_cs);
                await conn.OpenAsync();
                // ServerVersion te devolverá un string detallado (Ej: "WI-V3.0.7.33374 Firebird 3.0")
                System.Diagnostics.Debug.WriteLine($"Versión de Firebird: {conn.ServerVersion}");
                return true;
            }
            catch { return false; }
        }

        // ── Cargar partidas de una fecha ──────────────────────────────────────

        /// <param name="tabla">HISTORICO, o la tabla de importación del log (misma estructura).
        /// Si esa tabla todavía no existe, devuelve una lista vacía.</param>
        public async Task<List<HistoricoEntry>> CargarPorFechaAsync(
                DateTime fecha, int codSala, int codConc, string tabla = "HISTORICO") {
            var lista = new List<HistoricoEntry>();
            tabla = Gregal.Data.FilaHistorico.ValidarNombreTabla(tabla).ToUpperInvariant();
            if (tabla != "HISTORICO" && (await ObtenerColumnasAsync(tabla)).Count == 0) return lista;
            string sql = $@"SELECT COD_SALA, COD_CONC, FECHA_SESION, PARTIDA_DIA,
                                   FECHA, PRECIO, VENDIDOS, VENTA, PREMIO1, PREMIO2
                              FROM {tabla}
                             WHERE COD_SALA=@sala AND COD_CONC=@conc
                               AND FECHA_SESION=@fecha
                             ORDER BY PARTIDA_DIA";
            try {
                using var conn = new FbConnection(_cs);
                using var cmd  = new FbCommand(sql, conn);
                cmd.Parameters.Add("@sala",  FbDbType.Integer).Value = codSala;
                cmd.Parameters.Add("@conc",  FbDbType.Integer).Value = codConc;
                cmd.Parameters.Add("@fecha", FbDbType.Date).Value    = fecha.Date;

                await conn.OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync()) {
                    lista.Add(new HistoricoEntry {
                        CodSala    = rd.GetInt32(0),
                        CodConc    = rd.GetInt32(1),
                        FechaSesion= rd.GetDateTime(2),
                        PartidaDia = rd.GetInt32(3),
                        Fecha      = rd.IsDBNull(4) ? DateTime.MinValue : rd.GetDateTime(4),
                        Precio     = rd.IsDBNull(5) ? 0 : rd.GetDecimal(5),
                        Vendidos   = rd.IsDBNull(6) ? 0 : rd.GetInt32(6),
                        Venta      = rd.IsDBNull(7) ? 0 : rd.GetDecimal(7),
                        Premio1    = rd.IsDBNull(8) ? 0 : rd.GetDecimal(8),
                        Premio2    = rd.IsDBNull(9) ? 0 : rd.GetDecimal(9),
                    });
                }
            }
            catch (Exception ex) {
                throw new Exception($"Error al cargar partidas: {ex.Message}", ex);
            }
            return lista;
        }

        // ── Última fecha con datos ──────────────────────────────────────────────

        /// <summary>
        /// Devuelve la fecha de sesión más reciente con partidas registradas para
        /// la sala/concesionario indicados, o null si todavía no hay ninguna.
        /// </summary>
        public async Task<DateTime?> ObtenerUltimaFechaConDatosAsync(int codSala, int codConc) {
            const string sql = @"SELECT MAX(FECHA_SESION) FROM HISTORICO
                                   WHERE COD_SALA=@sala AND COD_CONC=@conc";
            try {
                using var conn = new FbConnection(_cs);
                using var cmd  = new FbCommand(sql, conn);
                cmd.Parameters.Add("@sala", FbDbType.Integer).Value = codSala;
                cmd.Parameters.Add("@conc", FbDbType.Integer).Value = codConc;

                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                return result is DateTime fecha ? fecha : null;
            }
            catch (Exception ex) {
                throw new Exception($"Error al obtener la última fecha con datos: {ex.Message}", ex);
            }
        }

        // ── Guardar (INSERT OR UPDATE) ────────────────────────────────────────

        public async Task GuardarAsync(HistoricoEntry e) {
            e.Venta = e.Precio * e.Vendidos;   // recalcular siempre

            const string sql = @"UPDATE OR INSERT INTO HISTORICO
                (COD_SALA, COD_CONC, FECHA_SESION, PARTIDA_DIA,
                 FECHA, PRECIO, VENDIDOS, VENTA, PREMIO1, PREMIO2)
                VALUES (@sala, @conc, @sesion, @pdia,
                        'NOW', @precio, @vendidos, @venta, @premio1, @premio2)
                MATCHING (COD_SALA, COD_CONC, FECHA_SESION, PARTIDA_DIA)";
            try {
                using var conn = new FbConnection(_cs);
                using var cmd  = new FbCommand(sql, conn);
                cmd.Parameters.Add("@sala",     FbDbType.Integer).Value = e.CodSala;
                cmd.Parameters.Add("@conc",     FbDbType.Integer).Value = e.CodConc;
                cmd.Parameters.Add("@sesion",   FbDbType.Date).Value    = e.FechaSesion.Date;
                cmd.Parameters.Add("@pdia",     FbDbType.Integer).Value = e.PartidaDia;
                cmd.Parameters.Add("@precio",   FbDbType.Decimal).Value = e.Precio;
                cmd.Parameters.Add("@vendidos", FbDbType.Integer).Value = e.Vendidos;
                cmd.Parameters.Add("@venta",    FbDbType.Decimal).Value = e.Venta;
                cmd.Parameters.Add("@premio1",  FbDbType.Decimal).Value = e.Premio1;
                cmd.Parameters.Add("@premio2",  FbDbType.Decimal).Value = e.Premio2;
                
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex) {
                throw new Exception($"Error al guardar partida {e.PartidaDia}: {ex.Message}", ex);
            }
        }

        // ── Guardar en lote (una sola conexión + transacción) ──────────────────

        /// <summary>
        /// Guarda varias partidas en una única conexión/transacción: o se guardan
        /// todas o, si alguna falla, no queda ninguna a medio guardar.
        /// </summary>
        public async Task GuardarLoteAsync(IEnumerable<HistoricoEntry> entradas) {
            const string sql = @"UPDATE OR INSERT INTO HISTORICO
                (COD_SALA, COD_CONC, FECHA_SESION, PARTIDA_DIA,
                 FECHA, PRECIO, VENDIDOS, VENTA, PREMIO1, PREMIO2)
                VALUES (@sala, @conc, @sesion, @pdia,
                        'NOW', @precio, @vendidos, @venta, @premio1, @premio2)
                MATCHING (COD_SALA, COD_CONC, FECHA_SESION, PARTIDA_DIA)";

            using var conn = new FbConnection(_cs);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try {
                foreach (var e in entradas) {
                    e.Venta = e.Precio * e.Vendidos;   // recalcular siempre

                    using var cmd = new FbCommand(sql, conn, tx);
                    cmd.Parameters.Add("@sala",     FbDbType.Integer).Value = e.CodSala;
                    cmd.Parameters.Add("@conc",     FbDbType.Integer).Value = e.CodConc;
                    cmd.Parameters.Add("@sesion",   FbDbType.Date).Value    = e.FechaSesion.Date;
                    cmd.Parameters.Add("@pdia",     FbDbType.Integer).Value = e.PartidaDia;
                    cmd.Parameters.Add("@precio",   FbDbType.Decimal).Value = e.Precio;
                    cmd.Parameters.Add("@vendidos", FbDbType.Integer).Value = e.Vendidos;
                    cmd.Parameters.Add("@venta",    FbDbType.Decimal).Value = e.Venta;
                    cmd.Parameters.Add("@premio1",  FbDbType.Decimal).Value = e.Premio1;
                    cmd.Parameters.Add("@premio2",  FbDbType.Decimal).Value = e.Premio2;

                    await cmd.ExecuteNonQueryAsync();
                }
                await tx.CommitAsync();
            }
            catch (Exception ex) {
                await tx.RollbackAsync();
                throw new Exception($"Error al guardar el lote (se ha revertido todo el lote): {ex.Message}", ex);
            }
        }

        // ── Eliminar ──────────────────────────────────────────────────────────
        public async Task EliminarAsync(HistoricoEntry e) {
            const string sql = @"DELETE FROM HISTORICO
                                  WHERE COD_SALA=@sala AND COD_CONC=@conc
                                    AND FECHA_SESION=@sesion AND PARTIDA_DIA=@pdia";
            try {
                using var conn = new FbConnection(_cs);
                using var cmd  = new FbCommand(sql, conn);
                cmd.Parameters.Add("@sala",   FbDbType.Integer).Value = e.CodSala;
                cmd.Parameters.Add("@conc",   FbDbType.Integer).Value = e.CodConc;
                cmd.Parameters.Add("@sesion", FbDbType.Date).Value    = e.FechaSesion.Date;
                cmd.Parameters.Add("@pdia",   FbDbType.Integer).Value = e.PartidaDia;

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex) {
                throw new Exception($"Error al eliminar partida {e.PartidaDia}: {ex.Message}", ex);
            }
        }

        public async Task<HashSet<int>> ObtenerPartidasExistentesEnRangoAsync(DateTime fecha, int codSala, int codConc, int inicio, int fin) {
            var existentes = new HashSet<int>();

            // CORREGIDO: Nombres exactos de columnas con guiones bajos tal como están en tu BD
            string query = @"
                SELECT PARTIDA_DIA 
                FROM HISTORICO 
                WHERE FECHA_SESION = @Fecha 
                  AND COD_SALA = @Sala 
                  AND COD_CONC = @Conc 
                  AND PARTIDA_DIA BETWEEN @Inicio AND @Fin
                ORDER BY PARTIDA_DIA ASC";
            try {
                // CORREGIDO: Usamos '_cs' que es tu variable de conexión real
                using (var conn = new FbConnection(_cs)) {
                    using (var cmd = new FbCommand(query, conn)) {
                        // Pasamos los parámetros tipados para evitar fallos de conversión
                        cmd.Parameters.Add("@Fecha", FbDbType.Date).Value = fecha.Date;
                        cmd.Parameters.Add("@Sala", FbDbType.Integer).Value = codSala;
                        cmd.Parameters.Add("@Conc", FbDbType.Integer).Value = codConc;
                        cmd.Parameters.Add("@Inicio", FbDbType.Integer).Value = inicio;
                        cmd.Parameters.Add("@Fin", FbDbType.Integer).Value = fin;

                        await conn.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync()) {
                            while (await reader.ReadAsync()) {
                                existentes.Add(reader.GetInt32(0)); // Añade el PARTIDA_DIA al HashSet
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception($"Error al buscar partidas existentes en el rango: {ex.Message}", ex);
            }

            return existentes;
        }

        // === AÑADIR AL FINAL DE LA CLASE FirebirdService ===

        /// <summary>
        /// Elimina en bloque todas las partidas de un rango (mucho más rápido)
        /// </summary>
        public async Task EliminarRangoAsync(DateTime fechaSesion, int codSala, int codConc, int inicio, int fin) {
            const string sql = @"
        DELETE FROM HISTORICO 
         WHERE COD_SALA = @sala 
           AND COD_CONC = @conc 
           AND FECHA_SESION = @fecha 
           AND PARTIDA_DIA BETWEEN @inicio AND @fin";

            try {
                using var conn = new FbConnection(_cs);
                using var cmd = new FbCommand(sql, conn);

                cmd.Parameters.Add("@sala", FbDbType.Integer).Value = codSala;
                cmd.Parameters.Add("@conc", FbDbType.Integer).Value = codConc;
                cmd.Parameters.Add("@fecha", FbDbType.Date).Value = fechaSesion.Date;
                cmd.Parameters.Add("@inicio", FbDbType.Integer).Value = inicio;
                cmd.Parameters.Add("@fin", FbDbType.Integer).Value = fin;

                await conn.OpenAsync();
                int eliminados = await cmd.ExecuteNonQueryAsync();

                // Opcional: loggear cantidad eliminada
                System.Diagnostics.Debug.WriteLine($"Eliminadas {eliminados} partidas en rango.");
            }
            catch (Exception ex) {
                throw new Exception($"Error al eliminar rango {inicio}-{fin}: {ex.Message}", ex);
            }
        }

        // ── Ficha completa de una partida (todas las columnas de HISTORICO) ────

        // Las columnas se leen de la propia BD (RDB$RELATION_FIELDS) y no de una
        // lista fija: cada sala puede tener alguna columna más o menos, y así la
        // ficha muestra exactamente las que tiene. Se leen una sola vez por
        // ejecución y tabla (el esquema no cambia con la aplicación abierta;
        // salvo al crear la tabla de importación, que borra su entrada).
        private readonly Dictionary<string, List<ColumnaHistorico>> _columnas = new(StringComparer.OrdinalIgnoreCase);

        // Los nombres se meten tal cual en el SQL (no se pueden parametrizar), así
        // que solo se aceptan identificadores simples sin comillas.
        private static readonly System.Text.RegularExpressions.Regex IdentificadorSimple =
            new(@"^[A-Z][A-Z0-9_$]*$", System.Text.RegularExpressions.RegexOptions.Compiled);

        public Task<IReadOnlyList<ColumnaHistorico>> ObtenerColumnasHistoricoAsync() => ObtenerColumnasAsync("HISTORICO");

        /// <summary>Columnas de una tabla; lista vacía si la tabla no existe.</summary>
        public async Task<IReadOnlyList<ColumnaHistorico>> ObtenerColumnasAsync(string tabla) {
            if (_columnas.TryGetValue(tabla, out var enCache)) return enCache;

            const string sql = @"SELECT TRIM(rf.RDB$FIELD_NAME), f.RDB$FIELD_TYPE, f.RDB$FIELD_SUB_TYPE,
                                        f.RDB$FIELD_SCALE, f.RDB$FIELD_PRECISION, f.RDB$CHARACTER_LENGTH,
                                        COALESCE(rf.RDB$NULL_FLAG, f.RDB$NULL_FLAG, 0),
                                        CASE WHEN f.RDB$COMPUTED_SOURCE IS NULL THEN 0 ELSE 1 END
                                   FROM RDB$RELATION_FIELDS rf
                                   JOIN RDB$FIELDS f ON f.RDB$FIELD_NAME = rf.RDB$FIELD_SOURCE
                                  WHERE rf.RDB$RELATION_NAME = @tabla
                                  ORDER BY rf.RDB$FIELD_POSITION";
            var columnas = new List<ColumnaHistorico>();
            try {
                using var conn = new FbConnection(_cs);
                using var cmd  = new FbCommand(sql, conn);
                cmd.Parameters.Add("@tabla", FbDbType.VarChar).Value = tabla.ToUpperInvariant();
                await conn.OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();
                while (await rd.ReadAsync()) {
                    columnas.Add(CrearColumna(
                        nombre:     rd.GetString(0),
                        tipo:       EnteroONulo(rd, 1) ?? 0,
                        subTipo:    EnteroONulo(rd, 2) ?? 0,
                        escala:     -(EnteroONulo(rd, 3) ?? 0),   // Firebird la guarda en negativo
                        precision:  EnteroONulo(rd, 4) ?? 0,
                        longitud:   EnteroONulo(rd, 5),
                        noNulo:     (EnteroONulo(rd, 6) ?? 0) == 1,
                        calculada:  (EnteroONulo(rd, 7) ?? 0) == 1));
                }
            }
            catch (Exception ex) {
                throw new Exception($"Error al leer las columnas de {tabla}: {ex.Message}", ex);
            }

            if (columnas.Count == 0) {
                if (tabla.Equals("HISTORICO", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("No se encuentra la tabla HISTORICO en la base de datos.");
                return columnas;   // no existe: no se guarda en caché, puede crearse después
            }

            _columnas[tabla] = columnas;
            return columnas;
        }

        private static int? EnteroONulo(FbDataReader rd, int i) =>
            rd.IsDBNull(i) ? null : Convert.ToInt32(rd.GetValue(i), CultureInfo.InvariantCulture);

        // Códigos de RDB$FIELDS.RDB$FIELD_TYPE.
        private const int FbSmallint = 7, FbInteger = 8, FbFloat = 10, FbDate = 12, FbTime = 13,
                          FbChar = 14, FbBigint = 16, FbBoolean = 23, FbDouble = 27, FbTimestamp = 35,
                          FbVarchar = 37, FbBlob = 261;

        private static ColumnaHistorico CrearColumna(string nombre, int tipo, int subTipo, int escala,
                                                     int precision, int? longitud, bool noNulo, bool calculada) {
            // Una columna calculada o con un nombre raro se muestra, pero no se edita.
            bool editable = !calculada && IdentificadorSimple.IsMatch(nombre);

            if (tipo is FbSmallint or FbInteger or FbBigint) {
                // Máximo del tipo entero que guarda el valor; con escala (NUMERIC)
                // y precisión declarada, el de la precisión: NUMERIC(15,5) → 10 cifras
                // enteras y 5 decimales.
                decimal maxBase = tipo switch { FbSmallint => short.MaxValue, FbInteger => int.MaxValue, _ => long.MaxValue };
                decimal factor  = Potencia10(escala);
                decimal maximo  = precision > 0 && precision < 28
                    ? Math.Min(maxBase, Potencia10(precision) - 1) / factor
                    : maxBase / factor;
                string declarado = escala > 0
                    ? $"NUMERIC({(precision > 0 ? precision : "?")},{escala})"
                    : tipo switch { FbSmallint => "SMALLINT", FbInteger => "INTEGER", _ => "BIGINT" };
                return new ColumnaHistorico {
                    Nombre = nombre, Tipo = !editable ? TipoColumna.NoEditable : escala > 0 ? TipoColumna.Decimal : TipoColumna.Entero,
                    Escala = escala, Minimo = -maximo, Maximo = maximo, AdmiteNulo = !noNulo, EsCalculada = calculada,
                    DescripcionTipo = calculada ? declarado + " (calculada)" : declarado,
                    TipoSql = calculada ? null : declarado.Replace("NUMERIC(?,", "NUMERIC(18,"),
                };
            }

            var (tipoColumna, declarada) = tipo switch {
                FbFloat     => (TipoColumna.Real, "FLOAT"),
                FbDouble    => (TipoColumna.Real, "DOUBLE PRECISION"),
                FbDate      => (TipoColumna.Fecha, "DATE"),
                FbTime      => (TipoColumna.Hora, "TIME"),
                FbTimestamp => (TipoColumna.FechaHora, "TIMESTAMP"),
                FbChar      => (TipoColumna.Texto, $"CHAR({longitud})"),
                FbVarchar   => (TipoColumna.Texto, $"VARCHAR({longitud})"),
                FbBlob when subTipo == 1 => (TipoColumna.Texto, "BLOB SUB_TYPE TEXT"),
                FbBlob      => (TipoColumna.NoEditable, "BLOB SUB_TYPE 0"),
                FbBoolean   => (TipoColumna.NoEditable, "BOOLEAN"),
                _           => (TipoColumna.NoEditable, $"tipo {tipo}"),
            };
            return new ColumnaHistorico {
                Nombre = nombre,
                Tipo = editable ? tipoColumna : TipoColumna.NoEditable,
                // ±1e15 para FLOAT/DOUBLE: de sobra para cualquier importe y dentro
                // del rango de un NumericUpDown.
                Minimo = tipoColumna == TipoColumna.Real ? -1_000_000_000_000_000m : 0,
                Maximo = tipoColumna == TipoColumna.Real ?  1_000_000_000_000_000m : 0,
                Escala = tipoColumna == TipoColumna.Real ? 4 : 0,
                LongitudMaxima = tipo is FbChar or FbVarchar ? longitud : null,
                EsChar = tipo == FbChar,
                EsBlob = tipo == FbBlob,
                AdmiteNulo = !noNulo,
                EsCalculada = calculada,
                DescripcionTipo = calculada ? declarada + " (calculada)" : declarada,
                TipoSql = calculada || declarada.StartsWith("tipo ") ? null : declarada,
            };
        }

        private static decimal Potencia10(int n) {
            decimal r = 1;
            for (int i = 0; i < n; i++) r *= 10;
            return r;
        }

        /// <summary>
        /// Lee todas las columnas de una partida. Devuelve null si ya no existe
        /// (la han borrado desde que se cargó la rejilla). Los valores vienen tal
        /// cual los da el proveedor de Firebird (int, decimal, string con los
        /// espacios de relleno de CHAR, DateTime, TimeSpan...) y null para NULL.
        /// </summary>
        public async Task<Dictionary<string, object?>?> CargarFilaCompletaAsync(ClaveHistorico clave, string tabla = "HISTORICO") {
            tabla = Gregal.Data.FilaHistorico.ValidarNombreTabla(tabla).ToUpperInvariant();
            // Columnas de HISTORICO que también tenga la tabla (la de importación es una copia).
            var enTabla = (await ObtenerColumnasAsync(tabla)).Select(c => c.Nombre).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var columnas = await ObtenerColumnasHistoricoAsync();
            var legibles = columnas.Where(c => IdentificadorSimple.IsMatch(c.Nombre) && enTabla.Contains(c.Nombre)).ToList();

            string sql = $@"SELECT {string.Join(", ", legibles.Select(c => c.Nombre))}
                              FROM {tabla}
                             WHERE COD_SALA=@sala AND COD_CONC=@conc
                               AND FECHA_SESION=@sesion AND PARTIDA_DIA=@pdia";
            try {
                using var conn = new FbConnection(_cs);
                using var cmd  = new FbCommand(sql, conn);
                AñadirParametrosClave(cmd, clave);

                await conn.OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();
                if (!await rd.ReadAsync()) return null;

                var fila = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < legibles.Count; i++) {
                    fila[legibles[i].Nombre] = rd.IsDBNull(i) ? null : rd.GetValue(i);
                }
                return fila;
            }
            catch (Exception ex) {
                throw new Exception($"Error al cargar la {clave}: {ex.Message}", ex);
            }
        }

        public async Task<bool> ExistePartidaAsync(ClaveHistorico clave) {
            using var conn = new FbConnection(_cs);
            await conn.OpenAsync();
            return await ExistePartidaAsync(conn, null, clave);
        }

        private static async Task<bool> ExistePartidaAsync(FbConnection conn, FbTransaction? tx, ClaveHistorico clave) {
            const string sql = @"SELECT COUNT(*) FROM HISTORICO
                                  WHERE COD_SALA=@sala AND COD_CONC=@conc
                                    AND FECHA_SESION=@sesion AND PARTIDA_DIA=@pdia";
            using var cmd = new FbCommand(sql, conn, tx);
            AñadirParametrosClave(cmd, clave);
            var result = await cmd.ExecuteScalarAsync();
            return result != null && result != DBNull.Value && Convert.ToInt32(result) > 0;
        }

        private static void AñadirParametrosClave(FbCommand cmd, ClaveHistorico clave) {
            cmd.Parameters.Add("@sala",   FbDbType.Integer).Value = clave.CodSala;
            cmd.Parameters.Add("@conc",   FbDbType.Integer).Value = clave.CodConc;
            cmd.Parameters.Add("@sesion", FbDbType.Date).Value    = clave.FechaSesion.Date;
            cmd.Parameters.Add("@pdia",   FbDbType.Integer).Value = clave.PartidaDia;
        }

        private const string MensajePartidaCambiada =
            "La partida ha cambiado en la base de datos desde que se abrió la ficha (o ya no existe). " +
            "No se ha guardado nada: cierra la ficha y vuelve a abrirla para ver los datos actuales.";

        /// <summary>
        /// Guarda los campos modificados en la ficha, todo en una transacción.
        ///
        /// Los campos normales van en un UPDATE que solo toca esas columnas y
        /// cuyo WHERE exige que cada una siga teniendo el valor que se leyó al
        /// abrir la ficha: si otro programa (o Gregal) la ha cambiado mientras
        /// tanto, no se pisa su cambio y se avisa.
        ///
        /// Si cambia algún campo de la clave, la partida se mueve copiando la
        /// fila con la clave nueva y borrando la original, en vez de con un UPDATE
        /// de la clave. Los triggers de la sala (HISTORICO_AI / HISTORICO_AD)
        /// apuntan en LOGREPLICACION cada clave insertada/modificada o borrada para
        /// la replicación: con un UPDATE solo quedaría apuntada la clave nueva y la
        /// antigua no se borraría nunca en el destino. Antes se comprueba, en la
        /// misma transacción, que no haya ya otra partida con la clave nueva.
        /// </summary>
        public async Task ActualizarCamposAsync(ClaveHistorico claveOriginal, IReadOnlyList<CambioCampo> cambios) {
            if (cambios.Count == 0) return;
            if (cambios.Any(c => !c.Columna.Editable))
                throw new InvalidOperationException("Hay campos que no se pueden modificar desde aquí.");

            var claveNueva   = AplicarCambiosDeClave(claveOriginal, cambios);
            var normales     = cambios.Where(c => !c.Columna.EsClave).ToList();

            using var conn = new FbConnection(_cs);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try {
                if (claveNueva != claveOriginal && await ExistePartidaAsync(conn, tx, claveNueva)) {
                    throw new InvalidOperationException($"Ya existe la {claveNueva}. No se ha guardado nada.");
                }

                if (normales.Count > 0) {
                    await ActualizarSiNoHaCambiadoAsync(conn, tx, claveOriginal, normales, MensajePartidaCambiada);
                }
                else if (!await ExistePartidaAsync(conn, tx, claveOriginal)) {
                    throw new InvalidOperationException(MensajePartidaCambiada);
                }

                if (claveNueva != claveOriginal) {
                    await MoverPartidaAsync(conn, tx, claveOriginal, claveNueva);
                }
                await tx.CommitAsync();
            }
            catch (Exception ex) {
                await tx.RollbackAsync();
                if (ex is InvalidOperationException) throw;
                throw new Exception($"Error al guardar la {claveOriginal}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// UPDATE de HISTORICO que solo toca las columnas de <paramref name="cambios"/>
        /// (ninguna de la clave) y solo si cada una sigue teniendo su valor
        /// <see cref="CambioCampo.Antes"/>; si no (o la partida ya no existe),
        /// lanza InvalidOperationException con <paramref name="siHaCambiado"/>.
        /// </summary>
        private static async Task ActualizarSiNoHaCambiadoAsync(FbConnection conn, FbTransaction tx, ClaveHistorico clave,
                                                                IReadOnlyList<CambioCampo> cambios, string siHaCambiado) {
            using var cmd = new FbCommand("", conn, tx);
            AñadirParametrosClave(cmd, clave);
            var sets  = new List<string>();
            var where = new List<string> { "COD_SALA=@sala", "COD_CONC=@conc", "FECHA_SESION=@sesion", "PARTIDA_DIA=@pdia" };
            for (int i = 0; i < cambios.Count; i++) {
                var col = cambios[i].Columna;
                sets.Add($"{col.Nombre}=@v{i}");
                AñadirParametro(cmd, $"@v{i}", col, cambios[i].Despues);
                if (!col.EsBlob) {   // un BLOB no se puede comparar así
                    where.Add($"{col.Nombre} IS NOT DISTINCT FROM @o{i}");
                    AñadirParametro(cmd, $"@o{i}", col, cambios[i].Antes);
                }
            }
            cmd.CommandText = $"UPDATE HISTORICO SET {string.Join(", ", sets)} WHERE {string.Join(" AND ", where)}";
            if (await cmd.ExecuteNonQueryAsync() != 1) throw new InvalidOperationException(siHaCambiado);
        }

        /// <summary>
        /// Copia la fila con la clave nueva (todas las columnas que se pueden
        /// escribir, tal cual) y borra la original. Ver ActualizarCamposAsync.
        /// </summary>
        private async Task MoverPartidaAsync(FbConnection conn, FbTransaction tx, ClaveHistorico origen, ClaveHistorico destino) {
            var columnas = (await ObtenerColumnasHistoricoAsync())
                .Where(c => !c.EsCalculada && IdentificadorSimple.IsMatch(c.Nombre))
                .ToList();
            var valores = columnas.Select(c => c.Nombre.ToUpperInvariant() switch {
                "COD_SALA"     => "@nsala",
                "COD_CONC"     => "@nconc",
                "FECHA_SESION" => "@nsesion",
                "PARTIDA_DIA"  => "@npdia",
                _ => c.Nombre,
            });

            using (var copiar = new FbCommand(
                $@"INSERT INTO HISTORICO ({string.Join(", ", columnas.Select(c => c.Nombre))})
                   SELECT {string.Join(", ", valores)} FROM HISTORICO
                    WHERE COD_SALA=@sala AND COD_CONC=@conc AND FECHA_SESION=@sesion AND PARTIDA_DIA=@pdia", conn, tx)) {
                AñadirParametrosClave(copiar, origen);
                copiar.Parameters.Add("@nsala",   FbDbType.Integer).Value = destino.CodSala;
                copiar.Parameters.Add("@nconc",   FbDbType.Integer).Value = destino.CodConc;
                copiar.Parameters.Add("@nsesion", FbDbType.Date).Value    = destino.FechaSesion.Date;
                copiar.Parameters.Add("@npdia",   FbDbType.Integer).Value = destino.PartidaDia;
                if (await copiar.ExecuteNonQueryAsync() != 1) throw new InvalidOperationException(MensajePartidaCambiada);
            }

            using var borrar = new FbCommand(
                @"DELETE FROM HISTORICO
                   WHERE COD_SALA=@sala AND COD_CONC=@conc AND FECHA_SESION=@sesion AND PARTIDA_DIA=@pdia", conn, tx);
            AñadirParametrosClave(borrar, origen);
            if (await borrar.ExecuteNonQueryAsync() != 1) throw new InvalidOperationException(MensajePartidaCambiada);
        }

        /// <summary>Clave que tendrá la partida después de aplicar los cambios.</summary>
        public static ClaveHistorico AplicarCambiosDeClave(ClaveHistorico clave, IEnumerable<CambioCampo> cambios) {
            foreach (var c in cambios.Where(c => c.Columna.EsClave && c.Despues != null)) {
                clave = c.Columna.Nombre.ToUpperInvariant() switch {
                    "COD_SALA"     => clave with { CodSala = Convert.ToInt32(c.Despues, CultureInfo.InvariantCulture) },
                    "COD_CONC"     => clave with { CodConc = Convert.ToInt32(c.Despues, CultureInfo.InvariantCulture) },
                    "FECHA_SESION" => clave with { FechaSesion = Convert.ToDateTime(c.Despues, CultureInfo.InvariantCulture).Date },
                    "PARTIDA_DIA"  => clave with { PartidaDia = Convert.ToInt32(c.Despues, CultureInfo.InvariantCulture) },
                    _ => clave,
                };
            }
            return clave;
        }

        private static void AñadirParametro(FbCommand cmd, string nombre, ColumnaHistorico col, object? valor) {
            var tipo = col.Tipo switch {
                TipoColumna.Entero    => FbDbType.BigInt,
                TipoColumna.Decimal   => FbDbType.Decimal,
                TipoColumna.Real      => FbDbType.Double,
                TipoColumna.Texto     => col.EsBlob ? FbDbType.Text : FbDbType.VarChar,
                TipoColumna.Fecha     => FbDbType.Date,
                TipoColumna.Hora      => FbDbType.Time,
                TipoColumna.FechaHora => FbDbType.TimeStamp,
                _ => throw new InvalidOperationException($"La columna {col.Nombre} no se puede modificar."),
            };
            cmd.Parameters.Add(nombre, tipo).Value = valor is null ? DBNull.Value : col.Tipo switch {
                TipoColumna.Entero    => Convert.ToInt64(valor, CultureInfo.InvariantCulture),
                TipoColumna.Decimal   => Convert.ToDecimal(valor, CultureInfo.InvariantCulture),
                TipoColumna.Real      => Convert.ToDouble(valor, CultureInfo.InvariantCulture),
                TipoColumna.Texto     => Convert.ToString(valor, CultureInfo.InvariantCulture) ?? "",
                TipoColumna.Fecha     => Convert.ToDateTime(valor, CultureInfo.InvariantCulture).Date,
                TipoColumna.Hora      => AHora(valor),
                _                     => Convert.ToDateTime(valor, CultureInfo.InvariantCulture),
            };
        }

        /// <summary>Un valor de una columna TIME, venga como venga del proveedor o de la ficha.</summary>
        public static TimeSpan AHora(object valor) => valor switch {
            TimeSpan t => t,
            TimeOnly t => t.ToTimeSpan(),
            DateTime d => d.TimeOfDay,
            _ => TimeSpan.Parse(Convert.ToString(valor, CultureInfo.InvariantCulture) ?? "", CultureInfo.InvariantCulture),
        };

        // ── Importar partidas de un log de Gregal a una tabla aparte ───────────

        public const string TablaImportacionPorDefecto = "HISTORICO_LOG";

        /// <summary>
        /// Crea la tabla de importación (si no existe) con las mismas columnas,
        /// tipos y clave primaria que HISTORICO, leídos de la propia BD. Sin
        /// triggers: lo importado no debe entrar en la replicación de la sala
        /// (LOGREPLICACION) ni en la auditoría de HISTORICO. Devuelve true si la
        /// ha creado.
        /// </summary>
        public async Task<bool> CrearTablaComoHistoricoAsync(string tabla) {
            tabla = Gregal.Data.FilaHistorico.ValidarNombreTabla(tabla).ToUpperInvariant();
            if (tabla == "HISTORICO") throw new InvalidOperationException("La tabla de importación no puede ser HISTORICO.");
            if (tabla.Length > 31) throw new InvalidOperationException($"El nombre de tabla '{tabla}' es demasiado largo (Firebird 2.5 admite 31 caracteres).");
            if ((await ObtenerColumnasAsync(tabla)).Count > 0) return false;

            var origen = (await ObtenerColumnasHistoricoAsync()).Where(c => !c.EsCalculada).ToList();
            var raras = origen.Where(c => c.TipoSql == null || !IdentificadorSimple.IsMatch(c.Nombre)).Select(c => c.Nombre).ToList();
            if (raras.Count > 0)
                throw new InvalidOperationException($"No se sabe crear una copia de estas columnas de HISTORICO: {string.Join(", ", raras)}.");

            string pk = "PK_" + tabla;
            string ddl = $"CREATE TABLE {tabla} (\n  " +
                string.Join(",\n  ", origen.Select(c => $"{c.Nombre} {c.TipoSql}{(c.AdmiteNulo && !c.EsClave ? "" : " NOT NULL")}")) +
                $",\n  {(pk.Length <= 31 ? $"CONSTRAINT {pk} " : "")}PRIMARY KEY (COD_SALA, COD_CONC, FECHA_SESION, PARTIDA_DIA))";
            try {
                using var conn = new FbConnection(_cs);
                await conn.OpenAsync();
                using var cmd = new FbCommand(ddl, conn);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex) {
                throw new Exception($"Error al crear la tabla {tabla}: {ex.Message}", ex);
            }
            _columnas.Remove(tabla);
            return true;
        }

        public sealed record ResultadoImportacion(int Nuevas, int Sobrescritas, int RepetidasEnElLog, int ConBolasRecortadas);

        /// <summary>
        /// Guarda las partidas en la tabla de importación con el mismo UPDATE OR
        /// INSERT y los mismos valores que usa Gregal (Gregal.Data.FilaHistorico):
        /// una partida que ya existe con esa clave se sobrescribe. Todo en una
        /// transacción: o se guardan todas o ninguna.
        /// </summary>
        public async Task<ResultadoImportacion> ImportarPartidasAsync(string tabla, IReadOnlyList<Gregal.Models.HistoricoDTO> partidas) {
            tabla = Gregal.Data.FilaHistorico.ValidarNombreTabla(tabla).ToUpperInvariant();
            var columnas = await ObtenerColumnasAsync(tabla);
            if (columnas.Count == 0) throw new InvalidOperationException($"No existe la tabla {tabla}.");

            var nombres = columnas.Select(c => c.Nombre).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var faltan = Gregal.Data.FilaHistorico.Columnas.Where(c => !nombres.Contains(c)).ToList();
            if (faltan.Count > 0)
                throw new InvalidOperationException($"A la tabla {tabla} le faltan columnas que escribe Gregal: {string.Join(", ", faltan)}.");
            int maxBolas = columnas.First(c => c.Nombre.Equals("BOLAS", StringComparison.OrdinalIgnoreCase)).LongitudMaxima ?? 0;

            string sqlGuardar = Gregal.Data.FilaHistorico.SqlUpdateOrInsert(tabla);
            string sqlExiste  = $@"SELECT COUNT(*) FROM {tabla}
                                    WHERE COD_SALA=@sala AND COD_CONC=@conc AND FECHA_SESION=@sesion AND PARTIDA_DIA=@pdia";

            int nuevas = 0, sobrescritas = 0, repetidas = 0, recortadas = 0;
            var vistas = new HashSet<ClaveHistorico>();

            using var conn = new FbConnection(_cs);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try {
                foreach (var dto in partidas) {
                    var clave = new ClaveHistorico(dto.CodSala, dto.CodConc,
                        DateTime.ParseExact(dto.FechaSesion, "yyyy-MM-dd", CultureInfo.InvariantCulture), dto.PartidaDia);

                    if (!vistas.Add(clave)) {
                        repetidas++;
                    }
                    else {
                        using var existe = new FbCommand(sqlExiste, conn, tx);
                        AñadirParametrosClave(existe, clave);
                        if (Convert.ToInt32(await existe.ExecuteScalarAsync()) > 0) sobrescritas++;
                        else nuevas++;
                    }

                    var (valores, descartadas) = Gregal.Data.FilaHistorico.Valores(dto, DateTime.Now, maxBolas);
                    if (descartadas > 0) recortadas++;

                    using var guardar = new FbCommand(sqlGuardar, conn, tx);
                    for (int i = 0; i < Gregal.Data.FilaHistorico.Columnas.Count; i++) {
                        guardar.Parameters.Add("@" + Gregal.Data.FilaHistorico.Columnas[i], valores[i]);
                    }
                    await guardar.ExecuteNonQueryAsync();
                }
                await tx.CommitAsync();
            }
            catch (Exception ex) {
                await tx.RollbackAsync();
                throw new Exception($"Error al guardar las partidas en {tabla} (no se ha guardado ninguna): {ex.Message}", ex);
            }
            return new ResultadoImportacion(nuevas, sobrescritas, repetidas, recortadas);
        }

        // ── Pasar partidas de la tabla de importación a HISTORICO ──────────────

        public enum EstadoComparacion { FaltaEnHistorico, Distinta, Igual }

        /// <summary>
        /// Un campo que no coincide. Hueco: en HISTORICO está vacío (NULL, 0 o
        /// texto vacío) y en la tabla del log tiene valor. Los valores son los
        /// crudos de la BD (EnHistorico sirve para comprobar al guardar que nadie
        /// lo ha cambiado desde la comparación).
        /// </summary>
        public sealed record DiferenciaCampo(ColumnaHistorico Columna, object? EnHistorico, object? EnLog, bool EsHueco);

        public sealed record ComparacionPartida(
            ClaveHistorico Clave, EstadoComparacion Estado,
            IReadOnlyDictionary<string, object?> FilaLog,
            IReadOnlyList<DiferenciaCampo> Diferencias) {
            public int Huecos => Diferencias.Count(d => d.EsHueco);
        }

        /// <summary>Días (FECHA_SESION) que hay en la tabla para la sala/concesionario, del más reciente al más antiguo.</summary>
        public async Task<List<(DateTime Fecha, int Partidas)>> ObtenerSesionesAsync(string tabla, int codSala, int codConc) {
            tabla = Gregal.Data.FilaHistorico.ValidarNombreTabla(tabla).ToUpperInvariant();
            var sesiones = new List<(DateTime, int)>();
            if ((await ObtenerColumnasAsync(tabla)).Count == 0) return sesiones;

            using var conn = new FbConnection(_cs);
            using var cmd  = new FbCommand($@"SELECT FECHA_SESION, COUNT(*) FROM {tabla}
                                               WHERE COD_SALA=@sala AND COD_CONC=@conc
                                               GROUP BY FECHA_SESION ORDER BY FECHA_SESION DESC", conn);
            cmd.Parameters.Add("@sala", FbDbType.Integer).Value = codSala;
            cmd.Parameters.Add("@conc", FbDbType.Integer).Value = codConc;
            await conn.OpenAsync();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync()) sesiones.Add((rd.GetDateTime(0), Convert.ToInt32(rd.GetValue(1))));
            return sesiones;
        }

        /// <summary>
        /// Compara, partida a partida, las de un día en la tabla del log con las
        /// de HISTORICO, en las columnas que tienen las dos (sin la clave). Las
        /// fechas y horas se comparan al segundo y los CHAR sin el relleno, igual
        /// que en la ficha (FichaPartidaCampos.Normalizar).
        /// </summary>
        public async Task<List<ComparacionPartida>> CompararConHistoricoAsync(string tablaLog, int codSala, int codConc, DateTime fecha) {
            tablaLog = Gregal.Data.FilaHistorico.ValidarNombreTabla(tablaLog).ToUpperInvariant();
            var enLog = (await ObtenerColumnasAsync(tablaLog)).Select(c => c.Nombre).ToHashSet(StringComparer.OrdinalIgnoreCase);
            if (enLog.Count == 0) throw new InvalidOperationException($"No existe la tabla {tablaLog}: importa antes un log de Gregal.");
            var columnas = (await ObtenerColumnasHistoricoAsync())
                .Where(c => c.Editable && !c.EsCalculada && enLog.Contains(c.Nombre))
                .ToList();

            using var conn = new FbConnection(_cs);
            await conn.OpenAsync();
            var filasLog  = await LeerFilasDelDiaAsync(conn, tablaLog, columnas, codSala, codConc, fecha);
            var filasHist = (await LeerFilasDelDiaAsync(conn, "HISTORICO", columnas, codSala, codConc, fecha))
                .ToDictionary(f => Convert.ToInt32(f["PARTIDA_DIA"], CultureInfo.InvariantCulture));

            var resultado = new List<ComparacionPartida>();
            foreach (var log in filasLog) {
                int partida = Convert.ToInt32(log["PARTIDA_DIA"], CultureInfo.InvariantCulture);
                var clave = new ClaveHistorico(codSala, codConc, fecha, partida);
                if (!filasHist.TryGetValue(partida, out var hist)) {
                    resultado.Add(new ComparacionPartida(clave, EstadoComparacion.FaltaEnHistorico, log, Array.Empty<DiferenciaCampo>()));
                    continue;
                }
                var difs = new List<DiferenciaCampo>();
                foreach (var col in columnas.Where(c => !c.EsClave)) {
                    var h = FichaPartidaCampos.Normalizar(col, hist[col.Nombre]);
                    var l = FichaPartidaCampos.Normalizar(col, log[col.Nombre]);
                    if (FichaPartidaCampos.Iguales(h, l)) continue;
                    difs.Add(new DiferenciaCampo(col, hist[col.Nombre], log[col.Nombre], EstaVacio(h) && !EstaVacio(l)));
                }
                resultado.Add(new ComparacionPartida(clave, difs.Count == 0 ? EstadoComparacion.Igual : EstadoComparacion.Distinta, log, difs));
            }
            return resultado;
        }

        private static bool EstaVacio(object? normalizado) =>
            normalizado is null || normalizado is decimal d && d == 0 || normalizado is string s && s.Length == 0;

        private static async Task<List<Dictionary<string, object?>>> LeerFilasDelDiaAsync(
                FbConnection conn, string tabla, IReadOnlyList<ColumnaHistorico> columnas, int codSala, int codConc, DateTime fecha) {
            using var cmd = new FbCommand($@"SELECT {string.Join(", ", columnas.Select(c => c.Nombre))} FROM {tabla}
                                              WHERE COD_SALA=@sala AND COD_CONC=@conc AND FECHA_SESION=@sesion
                                              ORDER BY PARTIDA_DIA", conn);
            cmd.Parameters.Add("@sala",   FbDbType.Integer).Value = codSala;
            cmd.Parameters.Add("@conc",   FbDbType.Integer).Value = codConc;
            cmd.Parameters.Add("@sesion", FbDbType.Date).Value    = fecha.Date;
            var filas = new List<Dictionary<string, object?>>();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync()) {
                var fila = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < columnas.Count; i++) fila[columnas[i].Nombre] = rd.IsDBNull(i) ? null : rd.GetValue(i);
                filas.Add(fila);
            }
            return filas;
        }

        public sealed record PasoAHistorico(ComparacionPartida Partida, bool SoloHuecos);
        public sealed record ResultadoPaso(int Añadidas, int Sustituidas, int ConHuecosRellenados, int SinHuecos);

        /// <summary>
        /// Escribe en HISTORICO lo que se comparó, todo en una transacción (o
        /// todas o ninguna). Una partida que faltaba se inserta con los valores
        /// de la tabla del log; una distinta se actualiza en sus campos distintos
        /// (o solo en los huecos), y solo si en HISTORICO siguen como estaban al
        /// comparar: Gregal puede estar guardando partidas de ese día. Pasa por
        /// los triggers de HISTORICO (replicación de la sala y auditoría), como
        /// cualquier otro cambio. La tabla del log no se toca.
        /// </summary>
        public async Task<ResultadoPaso> PasarAHistoricoAsync(IReadOnlyList<PasoAHistorico> pasos) {
            var tipos = (await ObtenerColumnasHistoricoAsync()).ToDictionary(c => c.Nombre, StringComparer.OrdinalIgnoreCase);
            int añadidas = 0, sustituidas = 0, huecos = 0, sinHuecos = 0;

            using var conn = new FbConnection(_cs);
            await conn.OpenAsync();
            using var tx = conn.BeginTransaction();
            try {
                foreach (var (p, soloHuecos) in pasos) {
                    string haCambiado = $"La {p.Clave} ha cambiado en HISTORICO desde que se comparó. " +
                                        "No se ha pasado nada: vuelve a comparar.";
                    switch (p.Estado) {
                        case EstadoComparacion.FaltaEnHistorico: {
                            if (await ExistePartidaAsync(conn, tx, p.Clave)) throw new InvalidOperationException(haCambiado);
                            var cols = p.FilaLog.Keys.Select(n => tipos[n]).ToList();
                            using var cmd = new FbCommand(
                                $"INSERT INTO HISTORICO ({string.Join(", ", cols.Select(c => c.Nombre))}) " +
                                $"VALUES ({string.Join(", ", cols.Select((_, i) => $"@p{i}"))})", conn, tx);
                            for (int i = 0; i < cols.Count; i++) AñadirParametro(cmd, $"@p{i}", cols[i], p.FilaLog[cols[i].Nombre]);
                            await cmd.ExecuteNonQueryAsync();
                            añadidas++;
                            break;
                        }
                        case EstadoComparacion.Distinta: {
                            var cambios = p.Diferencias.Where(d => !soloHuecos || d.EsHueco)
                                .Select(d => new CambioCampo(d.Columna, d.EnHistorico, d.EnLog)).ToList();
                            if (cambios.Count == 0) { sinHuecos++; break; }
                            await ActualizarSiNoHaCambiadoAsync(conn, tx, p.Clave, cambios, haCambiado);
                            if (soloHuecos) huecos++; else sustituidas++;
                            break;
                        }
                    }
                }
                await tx.CommitAsync();
            }
            catch (Exception ex) {
                await tx.RollbackAsync();
                if (ex is InvalidOperationException) throw;
                throw new Exception($"Error al pasar las partidas a HISTORICO (no se ha pasado ninguna): {ex.Message}", ex);
            }
            return new ResultadoPaso(añadidas, sustituidas, huecos, sinHuecos);
        }

        // ── Crear tabla de operaciones si no existe ────────────────────────────

        public async Task CrearTablaOperacionesAsync() {
            try {
                using var conn = new FbConnection(_cs);
                await conn.OpenAsync();

                // 1. Comprobar si TABLA existe (si no existe, result será null)
                // En Firebird 2.5, contar registros es más confiable que SELECT 1
                const string sqlCheckTabla = "SELECT COUNT(*) FROM RDB$RELATIONS WHERE TRIM(RDB$RELATION_NAME) = 'OPERACIONESBGEDITOR'";                
                using (var cmd = new FbCommand(sqlCheckTabla, conn)) {
                    var result = await cmd.ExecuteScalarAsync();
                    int count = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    if (count == 0) {
                        const string sqlTabla = @"CREATE TABLE operacionesBGEditor (
                            ID INTEGER PRIMARY KEY,
                            FECHA TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
                            USUARIO VARCHAR(100) NOT NULL,
                            OPERACION VARCHAR(100) NOT NULL,
                            DETALLES BLOB SUB_TYPE TEXT,
                            CREATED_AT TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                        )";
                        using var cmdCreateTable = new FbCommand(sqlTabla, conn);
                        await cmdCreateTable.ExecuteNonQueryAsync();
                        System.Diagnostics.Debug.WriteLine("✓ Tabla 'operacionesBGEditor' creada.");
                    }
                    else {
                        System.Diagnostics.Debug.WriteLine("✓ Tabla 'operacionesBGEditor' ya existe.");
                    }
                }

                // 2. Comprobar y crear GENERADOR
                const string sqlCheckGen = "SELECT COUNT(*) FROM RDB$GENERATORS WHERE TRIM(RDB$GENERATOR_NAME) = 'GEN_OPERACIONESBGEDITOR_ID'";
                using (var cmd = new FbCommand(sqlCheckGen, conn)) {
                    var result = await cmd.ExecuteScalarAsync();
                    int count = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    if (count == 0) {
                        const string sqlGen = "CREATE GENERATOR GEN_OPERACIONESBGEDITOR_ID;";
                        using var cmdCreateGen = new FbCommand(sqlGen, conn);
                        await cmdCreateGen.ExecuteNonQueryAsync();
                        System.Diagnostics.Debug.WriteLine("✓ Generador 'GEN_OPERACIONESBGEDITOR_ID' creado.");
                    }
                    else {
                        System.Diagnostics.Debug.WriteLine("✓ Generador 'GEN_OPERACIONESBGEDITOR_ID' ya existe.");
                    }
                }

                // 3. Comprobar y crear TRIGGER
                const string sqlCheckTrig = "SELECT COUNT(*) FROM RDB$TRIGGERS WHERE TRIM(RDB$TRIGGER_NAME) = 'BI_OPERACIONESBGEDITOR_ID'";
                using (var cmd = new FbCommand(sqlCheckTrig, conn)) {
                    var result = await cmd.ExecuteScalarAsync();
                    int count = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    if (count == 0) {
                        const string sqlTrig = @"CREATE TRIGGER BI_OPERACIONESBGEDITOR_ID FOR operacionesBGEditor
                            ACTIVE BEFORE INSERT POSITION 0
                            AS
                            BEGIN
                                IF (NEW.ID IS NULL OR NEW.ID = 0) THEN
                                    NEW.ID = GEN_ID(GEN_OPERACIONESBGEDITOR_ID, 1);
                            END";
                        using var cmdCreateTrig = new FbCommand(sqlTrig, conn);
                        await cmdCreateTrig.ExecuteNonQueryAsync();
                        System.Diagnostics.Debug.WriteLine("✓ Trigger 'BI_OPERACIONESBGEDITOR_ID' creado.");
                    }
                    else {
                        System.Diagnostics.Debug.WriteLine("✓ Trigger 'BI_OPERACIONESBGEDITOR_ID' ya existe.");
                    }
                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"✗ Error al crear tabla de operaciones: {ex.Message}");
            }
        }

        // ── Trigger de auditoría a nivel de BD sobre HISTORICO ──────────────────

        /// <summary>
        /// Crea (si no existe) un trigger AFTER INSERT/UPDATE/DELETE sobre HISTORICO
        /// que registra cada cambio directamente en operacionesBGEditor desde el
        /// propio motor de Firebird. A diferencia del registro que hace la
        /// aplicación (LogService), este salta siempre que cambie la tabla,
        /// venga el cambio de esta app o de cualquier otro cliente SQL
        /// (IBExpert, isql, etc.), porque vive dentro de la base de datos.
        /// Requiere que operacionesBGEditor ya exista (CrearTablaOperacionesAsync).
        /// </summary>
        public async Task CrearTriggerAuditoriaHistoricoAsync() {
            const string triggerName = "TRG_HISTORICO_AUDITORIA";
            try {
                using var conn = new FbConnection(_cs);
                await conn.OpenAsync();

                const string sqlCheck = "SELECT COUNT(*) FROM RDB$TRIGGERS WHERE TRIM(RDB$TRIGGER_NAME) = @nombre";
                using (var cmd = new FbCommand(sqlCheck, conn)) {
                    cmd.Parameters.Add("@nombre", FbDbType.VarChar).Value = triggerName;
                    var result = await cmd.ExecuteScalarAsync();
                    int count = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    if (count > 0) {
                        System.Diagnostics.Debug.WriteLine($"✓ Trigger '{triggerName}' ya existe.");
                        return;
                    }
                }

                const string sqlTrigger = @"
                    CREATE TRIGGER TRG_HISTORICO_AUDITORIA FOR HISTORICO
                    ACTIVE AFTER INSERT OR UPDATE OR DELETE POSITION 0
                    AS
                    DECLARE VARIABLE detalles VARCHAR(500);
                    BEGIN
                      IF (INSERTING) THEN
                      BEGIN
                        detalles = '{""cod_sala"":' || CAST(NEW.COD_SALA AS VARCHAR(10)) ||
                                   ',""cod_conc"":' || CAST(NEW.COD_CONC AS VARCHAR(10)) ||
                                   ',""fecha_sesion"":""' || CAST(NEW.FECHA_SESION AS VARCHAR(10)) || '""' ||
                                   ',""partida_dia"":' || CAST(NEW.PARTIDA_DIA AS VARCHAR(10)) ||
                                   ',""precio"":' || CAST(NEW.PRECIO AS VARCHAR(15)) ||
                                   ',""vendidos"":' || CAST(NEW.VENDIDOS AS VARCHAR(10)) ||
                                   ',""venta"":' || CAST(NEW.VENTA AS VARCHAR(15)) ||
                                   ',""premio1"":' || CAST(NEW.PREMIO1 AS VARCHAR(15)) ||
                                   ',""premio2"":' || CAST(NEW.PREMIO2 AS VARCHAR(15)) || '}';
                        INSERT INTO operacionesBGEditor (USUARIO, OPERACION, DETALLES)
                          VALUES (CURRENT_USER, 'DB_TRIGGER_INSERT', :detalles);
                      END
                      ELSE IF (UPDATING) THEN
                      BEGIN
                        detalles = '{""cod_sala"":' || CAST(NEW.COD_SALA AS VARCHAR(10)) ||
                                   ',""cod_conc"":' || CAST(NEW.COD_CONC AS VARCHAR(10)) ||
                                   ',""fecha_sesion"":""' || CAST(NEW.FECHA_SESION AS VARCHAR(10)) || '""' ||
                                   ',""partida_dia"":' || CAST(NEW.PARTIDA_DIA AS VARCHAR(10)) ||
                                   ',""precio_antes"":' || CAST(OLD.PRECIO AS VARCHAR(15)) ||
                                   ',""precio_despues"":' || CAST(NEW.PRECIO AS VARCHAR(15)) ||
                                   ',""vendidos_antes"":' || CAST(OLD.VENDIDOS AS VARCHAR(10)) ||
                                   ',""vendidos_despues"":' || CAST(NEW.VENDIDOS AS VARCHAR(10)) ||
                                   ',""premio1_antes"":' || CAST(OLD.PREMIO1 AS VARCHAR(15)) ||
                                   ',""premio1_despues"":' || CAST(NEW.PREMIO1 AS VARCHAR(15)) ||
                                   ',""premio2_antes"":' || CAST(OLD.PREMIO2 AS VARCHAR(15)) ||
                                   ',""premio2_despues"":' || CAST(NEW.PREMIO2 AS VARCHAR(15)) || '}';
                        INSERT INTO operacionesBGEditor (USUARIO, OPERACION, DETALLES)
                          VALUES (CURRENT_USER, 'DB_TRIGGER_UPDATE', :detalles);
                      END
                      ELSE IF (DELETING) THEN
                      BEGIN
                        detalles = '{""cod_sala"":' || CAST(OLD.COD_SALA AS VARCHAR(10)) ||
                                   ',""cod_conc"":' || CAST(OLD.COD_CONC AS VARCHAR(10)) ||
                                   ',""fecha_sesion"":""' || CAST(OLD.FECHA_SESION AS VARCHAR(10)) || '""' ||
                                   ',""partida_dia"":' || CAST(OLD.PARTIDA_DIA AS VARCHAR(10)) ||
                                   ',""precio"":' || CAST(OLD.PRECIO AS VARCHAR(15)) ||
                                   ',""vendidos"":' || CAST(OLD.VENDIDOS AS VARCHAR(10)) ||
                                   ',""venta"":' || CAST(OLD.VENTA AS VARCHAR(15)) ||
                                   ',""premio1"":' || CAST(OLD.PREMIO1 AS VARCHAR(15)) ||
                                   ',""premio2"":' || CAST(OLD.PREMIO2 AS VARCHAR(15)) || '}';
                        INSERT INTO operacionesBGEditor (USUARIO, OPERACION, DETALLES)
                          VALUES (CURRENT_USER, 'DB_TRIGGER_DELETE', :detalles);
                      END
                    END";
                using var cmdCreate = new FbCommand(sqlTrigger, conn);
                await cmdCreate.ExecuteNonQueryAsync();
                System.Diagnostics.Debug.WriteLine($"✓ Trigger '{triggerName}' creado.");
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"✗ Error al crear el trigger de auditoría de HISTORICO: {ex.Message}");
            }
        }

        // ── Insertar operación en la tabla de log ────────────────────────────

        public async Task InsertarOperacionAsync(string usuario, string operacion, string detalles = null) {
            const string sql = @"INSERT INTO operacionesBGEditor (USUARIO, OPERACION, DETALLES)
                                 VALUES (@usuario, @operacion, @detalles)";
            
            try {
                using var conn = new FbConnection(_cs);
                using var cmd = new FbCommand(sql, conn);
                cmd.Parameters.Add("@usuario", FbDbType.VarChar).Value = usuario ?? "SISTEMA";
                cmd.Parameters.Add("@operacion", FbDbType.VarChar).Value = operacion ?? "";
                cmd.Parameters.Add("@detalles", FbDbType.Text).Value = detalles ?? "";

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error al insertar operación: {ex.Message}");
                // No lanzar excepción, el log no debe interrumpir la operación principal
            }
        }

        // ── Consultar operaciones para auditoría ────────────────────────────
        /// <summary>
        /// Obtiene todas las operaciones registradas en la tabla
        /// </summary>
        public async Task<List<OperacionLog>> ObtenerOperacionesAsync(int? ultimasN = null) {
            var operaciones = new List<OperacionLog>();

            string sql = @"SELECT ID, FECHA, USUARIO, OPERACION, DETALLES 
                          FROM operacionesBGEditor 
                          ORDER BY ID DESC";

            // Si se especifica un límite, agregar ROWS
            if (ultimasN.HasValue && ultimasN.Value > 0) {
                sql = $"SELECT FIRST {ultimasN.Value} ID, FECHA, USUARIO, OPERACION, DETALLES FROM operacionesBGEditor ORDER BY ID DESC";
            }

            try {
                using var conn = new FbConnection(_cs);
                using var cmd = new FbCommand(sql, conn);

                await conn.OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();

                while (await rd.ReadAsync()) {
                    operaciones.Add(new OperacionLog {
                        ID = rd.GetInt32(0),
                        Fecha = rd.IsDBNull(1) ? DateTime.MinValue : rd.GetDateTime(1),
                        Usuario = rd.IsDBNull(2) ? "" : rd.GetString(2),
                        Operacion = rd.IsDBNull(3) ? "" : rd.GetString(3),
                        Detalles = rd.IsDBNull(4) ? "" : rd.GetString(4)
                    });
                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error al obtener operaciones: {ex.Message}");
            }

            return operaciones;
        }

        /// <summary>
        /// Obtiene el conteo total de operaciones registradas
        /// </summary>
        public async Task<int> ObtenerCountOperacionesAsync() {
            const string sql = "SELECT COUNT(*) FROM operacionesBGEditor";

            try {
                using var conn = new FbConnection(_cs);
                using var cmd = new FbCommand(sql, conn);

                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error al contar operaciones: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Obtiene operaciones de un usuario específico
        /// </summary>
        public async Task<List<OperacionLog>> ObtenerOperacionesPorUsuarioAsync(string usuario, int? ultimasN = null) {
            var operaciones = new List<OperacionLog>();

            string limitClause = ultimasN.HasValue && ultimasN.Value > 0 ? $"FIRST {ultimasN.Value} " : "";
            string sql = $@"SELECT {limitClause}ID, FECHA, USUARIO, OPERACION, DETALLES 
                          FROM operacionesBGEditor 
                          WHERE USUARIO = @usuario 
                          ORDER BY ID DESC";
            try {
                using var conn = new FbConnection(_cs);
                using var cmd = new FbCommand(sql, conn);
                cmd.Parameters.Add("@usuario", FbDbType.VarChar).Value = usuario ?? "";

                await conn.OpenAsync();
                using var rd = await cmd.ExecuteReaderAsync();

                while (await rd.ReadAsync()) {
                    operaciones.Add(new OperacionLog {
                        ID = rd.GetInt32(0),
                        Fecha = rd.IsDBNull(1) ? DateTime.MinValue : rd.GetDateTime(1),
                        Usuario = rd.IsDBNull(2) ? "" : rd.GetString(2),
                        Operacion = rd.IsDBNull(3) ? "" : rd.GetString(3),
                        Detalles = rd.IsDBNull(4) ? "" : rd.GetString(4)
                    });
                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error al obtener operaciones del usuario: {ex.Message}");
            }
            return operaciones;
        }
    }
}

