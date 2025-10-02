using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using backend_ont_2.Xmltxt.DataClass;



namespace backend_ont_2.OracleDbProject
{
    public class OracleDb : IDisposable
    {
        private readonly string _readOnlyConnectionString;
        private readonly string _writeConnectionString;
        private OracleConnection? _connection;
        private bool _disposed = false;

        public OracleDb(IConfiguration configuration)
        {
            _readOnlyConnectionString = configuration["Oracle:User1:ConnectionString"]
                 ?? throw new InvalidOperationException("Falta la cadena de conexión Oracle:User1:ConnectionString");

            _writeConnectionString = configuration["Oracle:User2:ConnectionString"]
                 ?? throw new InvalidOperationException("Falta la cadena de conexión Oracle:User2:ConnectionString");

            Console.WriteLine($"{_readOnlyConnectionString}  {_writeConnectionString}");
        }

        // ✅ Métodos de conexión (Open no tiene OpenAsync en ODP.NET Managed, pero puedes usar Task.Run si es bloqueante)
        public void ConnectAsReadOnly()
        {
            Connect(_readOnlyConnectionString, "User1 (readOnly)");
        }

        public void ConnectAsWrite()
        {
            Connect(_writeConnectionString, "User2 (write)");
        }

        private void Connect(string connectionString, string userType)
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                Console.WriteLine($"✅ Ya estás conectado ({userType}).");
                return;
            }

            try
            {
                Console.WriteLine($"🔌 Conectando a Oracle ({userType} {connectionString})...");
                _connection = new OracleConnection(connectionString);
                _connection.Open();
                Console.WriteLine("✅ Conexión establecida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al conectar como {userType}: {ex.Message}");
                _connection = null;
                throw;
            }
        }

        // ✅ Ejecutar consulta asíncrona con conexión actual
        public async Task<List<Dictionary<string, object?>>> ExecuteQuery(string query)
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                Console.WriteLine("❌ Primero debes establecer la conexión (ConnectAsReadOnly o ConnectAsWrite).");
                return new List<Dictionary<string, object?>>();
            }

            var results = new List<Dictionary<string, object?>>();

            try
            {
                using var cmd = new OracleCommand(query, _connection);
                cmd.FetchSize = 10000; // 🔼 Mejora de rendimiento para grandes volúmenes
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        row[columnName] = value;
                    }
                    results.Add(row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al ejecutar consulta: {ex.Message}");
                if (ex is OracleException oracleEx)
                {
                    Console.WriteLine($"    Código Oracle: {oracleEx.Number}");
                }
                Console.WriteLine($"    Query: {query}");
            }

            return results;
        }

        // ✅ Ejecutar consulta de solo lectura (asíncrona)

        public async Task<List<Dictionary<string, object?>>> QueryReadOnly(string query)
        {
            return await ExecuteWithConnectionAsync(_readOnlyConnectionString, query, "User1 (readOnly)");
        }

        // ✅ Ejecutar consulta de escritura (asíncrona)
        public async Task<List<Dictionary<string, object?>>> QueryWrite(string query)
        {
            return await ExecuteWithConnectionAsync(_writeConnectionString, query, "User2 (write)");
        }

        /***********************************************************************************************************************/
// ✅ Método para INSERT con conexión temporal (similar a ExecuteWithConnectionAsync)
        public async Task<int> ExecuteNonQueryWithConnectionAsync(string connStr, string sql, string context)
        {
            try
            {
                using var connection = new OracleConnection(connStr);
                await connection.OpenAsync();
                Console.WriteLine($"✅ Conexión temporal abierta ({context})");

                using var cmd = new OracleCommand(sql, connection);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                
                Console.WriteLine($"✅ Comando ejecutado ({context}). Filas afectadas: {rowsAffected}");
                return rowsAffected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al ejecutar comando ({context}): {ex.Message}");
                if (ex is OracleException oracleEx)
                {
                    Console.WriteLine($"    Código Oracle: {oracleEx.Number}");
                }
                Console.WriteLine($"    SQL: {sql}");
                return 0;
            }
        }

       public async Task<int> InsertTxtSeniatAsync(TxtSeniat txt)
        {
            const string sql = @"INSERT INTO TXT_SENIAT 
                                (ORGA_ID, INFN_CODIGO, AGENCIA_CODIGO, IDENT_CNTB, PLANILLA, FECHA_RECAUDACION, 
                                TIPO_TRANSACCION, FORMA_CODIGO, MONTO_EFECTIVO, MONTO_OTROS_PAGOS, COD_SEGURIDAD, 
                                SAFE, ESTADO, ANHO, LOTE_SEQ, PLAN_SEQ) 
                                VALUES 
                                (:OrgaId, :InfnCodigo, :AgenciaCodigo, :IdentCntb, :Planilla, TO_DATE(:FechaRecaudacion, 'DD/MM/YYYY'), 
                                :TipoTransaccion, :FormaCodigo, :MontoEfectivo, :MontoOtrosPagos, :CodSeguridad, 
                                :Safe, :Estado, :Anho, :LoteSeq, :PlanSeq)";

            try
            {
                using var connection = new OracleConnection(_writeConnectionString);
                await connection.OpenAsync();
                Console.WriteLine("✅ Conexión temporal abierta (User2 - write)");
                
                using var cmd = new OracleCommand(sql, connection);
                
                // Formatear fecha como string en formato DD/MM/YYYY
                string fechaFormateada = txt.FechaRecaudacion.ToString("dd/MM/yyyy");
                
                // Agregar parámetros con los nombres correctos
                cmd.Parameters.Add(":OrgaId", OracleDbType.Varchar2).Value = txt.Organismo ?? (object)DBNull.Value;
                cmd.Parameters.Add(":InfnCodigo", OracleDbType.Varchar2).Value = txt.Banco ?? (object)DBNull.Value;
                cmd.Parameters.Add(":AgenciaCodigo", OracleDbType.Varchar2).Value = txt.Agencia ?? (object)DBNull.Value;
                cmd.Parameters.Add(":IdentCntb", OracleDbType.Varchar2).Value = txt.Rif ?? (object)DBNull.Value;
                cmd.Parameters.Add(":Planilla", OracleDbType.Varchar2).Value = txt.Planilla ?? (object)DBNull.Value;
                cmd.Parameters.Add(":FechaRecaudacion", OracleDbType.Varchar2).Value = fechaFormateada;
                cmd.Parameters.Add(":TipoTransaccion", OracleDbType.Int32).Value = txt.TipoTransaccion ?? (object)DBNull.Value;
                cmd.Parameters.Add(":FormaCodigo", OracleDbType.Varchar2).Value = txt.Forma ?? (object)DBNull.Value;
                cmd.Parameters.Add(":MontoEfectivo", OracleDbType.Double).Value = txt.Efectivo ?? (object)DBNull.Value;
                cmd.Parameters.Add(":MontoOtrosPagos", OracleDbType.Double).Value = txt.OtrosPagos ?? (object)DBNull.Value;
                cmd.Parameters.Add(":CodSeguridad", OracleDbType.Varchar2).Value = txt.Seguridad ?? (object)DBNull.Value;
                cmd.Parameters.Add(":Safe", OracleDbType.Varchar2).Value = txt.Safe ?? (object)DBNull.Value;
                cmd.Parameters.Add(":Estado", OracleDbType.Int32).Value = txt.Estado ?? (object)DBNull.Value;
                cmd.Parameters.Add(":Anho", OracleDbType.Int32).Value = txt.Anho ?? (object)DBNull.Value;
                cmd.Parameters.Add(":LoteSeq", OracleDbType.Int32).Value = txt.LoteSeq ?? (object)DBNull.Value;
                cmd.Parameters.Add(":PlanSeq", OracleDbType.Int32).Value = txt.PlanSeq ?? (object)DBNull.Value;

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"✅ TxtSeniat insertado correctamente. Filas afectadas: {rowsAffected}");
                return rowsAffected;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al insertar TxtSeniat: {ex.Message}");
                return 0;
            }
        }
        /***************************************************************************************************************************/


        // ✅ Método auxiliar asíncrono para ejecutar sin mantener conexión abierta
        private async Task<List<Dictionary<string, object?>>> ExecuteWithConnectionAsync(string connStr, string query, string context)
        {
            var results = new List<Dictionary<string, object?>>();

            try
            {
                using var connection = new OracleConnection(connStr);
                await connection.OpenAsync();
                Console.WriteLine($"✅ Conexión temporal abierta ({context})");

                using var cmd = new OracleCommand(query, connection);
                cmd.FetchSize = 10000;
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        row[columnName] = value;
                    }
                    results.Add(row);
                }

                Console.WriteLine($"✅ Consulta ejecutada ({context})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al ejecutar consulta ({context}): {ex.Message}");
                if (ex is OracleException oracleEx)
                {
                    Console.WriteLine($"    Código Oracle: {oracleEx.Number}");
                }
                Console.WriteLine($"    Query: {query}");
            }

            return results;
        }


        // ✅ Cerrar conexión
        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_connection != null)
                {
                    try
                    {
                        if (_connection.State == ConnectionState.Open)
                        {
                            _connection.Close();
                            Console.WriteLine("🔌 Conexión cerrada.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error al cerrar conexión: {ex.Message}");
                    }
                    finally
                    {
                        _connection.Dispose();
                    }
                }
                _connection = null;
                _disposed = true;
            }
        }
    }
}
 /*
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace backend_ont_2.OracleDbProject // ← Ajusta el namespace según tu proyecto
{
    public class OracleDb : IDisposable
    {
        private OracleConnection? _connection;
        private bool _disposed = false;


        
        public void Connect(string dsn, string user, string password)
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                Console.WriteLine("✅ Ya estás conectado.");
                return;
            }

            try
            {
                Console.WriteLine("🔌 Conectando a la base de datos...");
                string connectionString = $"User Id={user};Password={password};Data Source={dsn};";
                _connection = new OracleConnection(connectionString);
                _connection.Open();
                Console.WriteLine("✅ Conexión establecida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al conectar: {ex.Message}");
                _connection = null;
                throw;
            }
        }

        public async Task<List<Dictionary<string, object?>>> ExecuteQuery(string query)
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                Console.WriteLine("❌ Primero debes establecer la conexión");
                return new List<Dictionary<string, object?>>();
            }

            var results = new List<Dictionary<string, object?>>();

            try
            {
                using var cmd = new OracleCommand(query, _connection);
                using var reader = await cmd.ExecuteReaderAsync(); // ← Asíncrono aquí

                while (await reader.ReadAsync()) // ← Y aquí
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object? value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        row[columnName] = value;
                    }
                    results.Add(row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al ejecutar consulta: {ex.Message}");
                if (ex is OracleException oracleEx)
                {
                    Console.WriteLine($"    SQL Code: {oracleEx.Number}");
                }
                Console.WriteLine($"    Query: {query}");
            }

            return results;
        }

 

         

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_connection != null)
                {
                    try
                    {
                        if (_connection.State == ConnectionState.Open)
                        {
                            _connection.Close();
                            Console.WriteLine("🔌 Conexión cerrada.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error al cerrar conexión: {ex.Message}");
                    }
                    finally
                    {
                        _connection.Dispose();
                    }
                }
                _connection = null;
                _disposed = true;
            }
        }
    }
}  */
