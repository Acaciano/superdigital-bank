using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SuperDigital.DigitalAccount.Data.Repositories.DB
{
    public class DbSQLManager
    {
        private string _connectionString = string.Empty;
        private UnitOfWork _unitOfWork;

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
        public UnitOfWork UnitOfWork { get; set; }

        public DbSQLManager(string connectionString)
        {
            this._connectionString = connectionString;

        }

        public DbSQLManager(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

        }

        public T GetFirsOrDefault<T>(string query, object queryParam = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            if (UnitOfWork != null)
                return GetFirsOrDefaultTransaction<T>(query, queryParam);
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    return dbConnection.QueryFirstOrDefault<T>(query, queryParam);
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception("Erro ao realizar consulta no banco de dados", ex);
            }
            finally
            {
                timer.Stop();
            }
        }

        private T GetFirsOrDefaultTransaction<T>(string query, object queryParam = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                using (var command = UnitOfWork.CreateCommand())
                {
                    var value = command.Connection.QueryFirstOrDefault<T>(query, queryParam, command.Transaction);
                    return value;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }

        }

        public long ExecuteCommand(string query, object queryParam = null, int? commandTimeout = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            if (UnitOfWork != null)
                return ExecuteCommandTransaction(query, queryParam, null, null, commandTimeout);

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();

                    var t = dbConnection.BeginTransaction();
                    try
                    {
                        var value = dbConnection.Execute(query, queryParam, t, commandTimeout: commandTimeout);
                        t.Commit();
                        return value;
                    }
                    catch (Exception ex)
                    {
                        t.Rollback();
                        throw ex;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }
        }

        public long ExecuteCommandNotTransaction(string query, object queryParam = null, int? commandTimeout = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            if (UnitOfWork != null)
                return ExecuteCommandTransaction(query, queryParam, null, null, commandTimeout);

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    
                    try
                    {
                        var value = dbConnection.Execute(query, queryParam, null, commandTimeout: commandTimeout);
                        return value;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }
        }

        public object ExecuteScalar(string query, object queryParam = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            if (UnitOfWork != null)
                return ExecuteScalarTransaction(query, queryParam);

            try
            {
                using (IDbConnection dbConnection = Connection)
                {

                    dbConnection.Open();

                    var t = dbConnection.BeginTransaction();
                    try
                    {

                        var value = dbConnection.ExecuteScalar(query, queryParam, t);
                        t.Commit();
                        return value;
                    }
                    catch (Exception ex)
                    {
                        t.Rollback();
                        throw ex;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }
        }

        private int ExecuteCommandTransaction(string query, object queryParam = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", int? commandTimeout = null)
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                using (var command = UnitOfWork.CreateCommand())
                {
                    var value = command.Connection.Execute(query, queryParam, command.Transaction, commandTimeout);
                    return value;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }
        }

        private object ExecuteScalarTransaction(string query, object queryParam = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                using (var command = UnitOfWork.CreateCommand())
                {
                    var value = command.Connection.ExecuteScalar(query, queryParam, command.Transaction);
                    return value;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }
        }

        private IEnumerable<T> GetListTransaction<T>(string query, object queryParam = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                using (var command = UnitOfWork.CreateCommand())
                {
                    var value = command.Connection.Query<T>(query, queryParam, command.Transaction);
                    return value;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }

        }

        public IEnumerable<T> GetList<T>(string query, object queryParam = null, int? commandTimeout = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            if (UnitOfWork != null)
                return GetListTransaction<T>(query, queryParam);

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    return dbConnection.Query<T>(query, queryParam, commandTimeout: commandTimeout);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }

        }

        public IEnumerable<dynamic> GetDynamic(string query, List<SqlParameter> parameters = null, int? commandTimeout = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var result = new List<dynamic>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = connection.CreateCommand();

                    if (commandTimeout.HasValue)
                        cmd.CommandTimeout = commandTimeout.Value;

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader == null)
                            return null;

                        var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                        while (reader.Read())
                        {
                            var obj = new ExpandoObject() as IDictionary<string, object>;

                            for (int i = 0; i < columns.Count; i++)
                            {
                                obj.Add(columns[i], reader[i]);
                            }

                            result.Add(obj);
                        }
                    }
                }


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }
        }

        public string GetResultAsJsonTransaction(string sqlQuery, bool forJsonAuto = true, bool withoutArrayWrapper = true, List<SqlParameter> parameters = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                string result = string.Empty;
                //using (var connection = new SqlConnection(connectionString))
                using (var cmd = UnitOfWork.CreateCommand())
                {
                    //var cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;
                    if (parameters != null)
                        foreach (var parameter in parameters)
                            cmd.Parameters.Add(parameter);

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result += $"{reader[0]}";
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                timer.Stop();
            }
        }

        /// <summary>
        ///  Return from SQL DataBase the result of Query as JsonString
        /// </summary>
        /// <param name="sqlQuery">Query</param>
        /// <param name="whitoutArrayWrapper">If true, remove the wrapper [] from jsonstring</param>
        /// <returns>Query result  as JSON string</returns>
        public string GetResultAsJson(string sqlQuery, bool forJsonAuto = true, bool withoutArrayWrapper = true, List<SqlParameter> parameters = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            string result = string.Empty;
            try
            {

                if (forJsonAuto)
                    sqlQuery += "   FOR JSON AUTO ";

                if (withoutArrayWrapper)
                    sqlQuery += ", WITHOUT_ARRAY_WRAPPER";

                if (UnitOfWork != null)
                    return GetResultAsJsonTransaction(sqlQuery, forJsonAuto, withoutArrayWrapper, parameters);

                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters.ToArray());


                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result += $"{reader[0]}";

                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                timer.Stop();
            }
        }

        public SqlMapper.GridReader GetDataReader(string query, object parameters = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var con = new SqlConnection(_connectionString);
                con.Open();

                SqlMapper.GridReader result = con.QueryMultiple(query, param: parameters, commandType: CommandType.StoredProcedure, commandTimeout: (3 * 60));
                return result;
            }
            catch (Exception)
            {

                throw;
            }

            finally
            {
                timer.Stop();
            }
        }

        public IEnumerable<T> GetListStoredProcedure<T>(string query, object queryParam = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            if (UnitOfWork != null)
                return GetListTransaction<T>(query, queryParam);

            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    return dbConnection.Query<T>(query, queryParam, commandType: CommandType.StoredProcedure);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            finally
            {
                timer.Stop();
            }
        }
    }
}