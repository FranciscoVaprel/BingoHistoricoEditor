using BingoHistoricoEditor.Models;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using FirebirdSql.Data.FirebirdClient;

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

        public async Task<bool> TestConnectionAsync() {
            try {
                using var conn = new FbConnection(_cs);
                await conn.OpenAsync();
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
    }
}
