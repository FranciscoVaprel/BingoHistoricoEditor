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

        public async Task<List<HistoricoEntry>> CargarPorFechaAsync(
                DateTime fecha, int codSala, int codConc) {
            var lista = new List<HistoricoEntry>();
            const string sql = @"SELECT COD_SALA, COD_CONC, FECHA_SESION, PARTIDA_DIA,
                                        FECHA, PRECIO, VENDIDOS, VENTA, PREMIO1, PREMIO2
                                   FROM HISTORICO
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

