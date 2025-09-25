using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

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
