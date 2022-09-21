using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Web.Helpers
{
    /// <summary>
    /// An ADO.NET helper class
    /// http://www.blackbeltcoder.com/Articles/ado/an-ado-net-sql-helper-class
    /// http://www.totaldotnet.com/sub/custom-sqlhelper-class-in-csharp-for-your-database-interaction
    /// https://psycodedeveloper.wordpress.com/2013/03/05/using-async-code-in-net-4-0/
    /// https://www.snip2code.com/Snippet/171371/SqlHelper-T--class-that-is-an-asynch-hel
    /// http://stackoverflow.com/questions/25681102/using-automapper-with-data-reader
    /// </summary>
    public class SqlHelper : IDisposable
    {
        // Internal members
        protected string _connString = null;
        protected SqlConnection _conn = null;
        protected SqlTransaction _trans = null;
        protected bool _disposed = false;
        protected static int commandTimeout = 0;

        public static int CommandTimeout
        {
            get { return SqlHelper.commandTimeout; }
        }

        /// <summary>
        /// Sets or returns the connection string use by all instances of this class.
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// Returns the current SqlTransaction object or null if no transaction
        /// is in effect.
        /// </summary>
        public SqlTransaction Transaction { get { return _trans; } }

        /// <summary>
        /// Constructor using global connection string.
        /// </summary>
        public SqlHelper()
        {
            _connString = ConnectionString;
            Connect();
        }

        /// <summary>
        /// Constructure using connection string override
        /// </summary>
        /// <param name="connString">Connection string for this instance</param>
        public SqlHelper(string connString)
        {
            //_connString = connString;
            _connString = ConfigurationManager.ConnectionStrings[connString].ConnectionString;
            //Connect();
        }

        // Creates a SqlConnection using the current connection string
        protected SqlConnection Connect()
        {
            _conn = new SqlConnection(_connString);
            _conn.Open();
            return _conn;
        }

        /// <summary>
        /// Constructs a SqlCommand with the given parameters. This method is normally called
        /// from the other methods and not called directly. But here it is if you need access
        /// to it.
        /// </summary>
        /// <param name="qry">SQL query or stored procedure name</param>
        /// <param name="type">Type of SQL command</param>
        /// <param name="args">Query arguments. Arguments should be in pairs where one is the
        /// name of the parameter and the second is the value. The very last argument can
        /// optionally be a SqlParameter object for specifying a custom argument type</param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string qry, CommandType type, params object[] args)
        {
            SqlCommand cmd = new SqlCommand(qry, _conn);

            // Associate with current transaction, if any
            if (_trans != null)
                cmd.Transaction = _trans;

            // Set command type
            cmd.CommandType = type;

            // Construct SQL parameters
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string && i < (args.Length - 1))
                {
                    SqlParameter parm = new SqlParameter();
                    parm.ParameterName = (string)args[i];
                    parm.Value = args[++i];
                    cmd.Parameters.Add(parm);
                }
                else if (args[i] is SqlParameter)
                {
                    cmd.Parameters.Add((SqlParameter)args[i]);

                    //EXAMPLE
                    //// Set Input Parameter
                    //SqlParameter oParam = new SqlParameter("@Username", oUsername);
                    //oParam.SqlDbType = SqlDbType.VarChar;
                    //cmd.Parameters.Add(oParam);

                    //// Set Output Paramater
                    //SqlParameter OutputParam = new SqlParameter("@OutputParam", SqlDbType.VarChar);
                    //OutputParam.Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add(OutputParam);

                    //// Set Input Output Parameter
                    //SqlParameter InOutParam = new SqlParameter("@InputOutputParam", InOutParamValue);
                    //InOutParam.SqlDbType = SqlDbType.Int;
                    //InOutParam.Direction = ParameterDirection.InputOutput;
                    //cmd.Parameters.Add(InOutParam);

                    //// Set ReturnValue Parameter
                    //SqlParameter RetParam = new SqlParameter("ReturnValue", DBNull.Value);
                    //RetParam.Direction = ParameterDirection.ReturnValue;
                    //cmd.Parameters.Add(RetParam);
                }
                else throw new ArgumentException("Invalid number or type of arguments supplied");
            }
            return cmd;
        }     

        #region Exec Members

       
        /// <summary>
        /// Executes a query that returns no results
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>The number of rows affected</returns>
        public int ExecNonQuery(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    cmd.CommandTimeout = 0;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task<int> ExecNonQueryAsync(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    cmd.CommandTimeout = 0;
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        /// <summary>
        /// Executes a stored procedure that returns no results
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>The number of rows affected</returns>
        public int ExecNonQueryProc(string proc, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(proc, CommandType.StoredProcedure, args))
                {
                    cmd.CommandTimeout = 0;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public async Task<int> ExecNonQueryProcAsync(string proc, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(proc, CommandType.StoredProcedure, args))
                {
                    cmd.CommandTimeout = 0;
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public object ExecScalar(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    return cmd.ExecuteScalar();
                }
            }
        }

        public async Task<object> ExecScalarAsync(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    cmd.CommandTimeout = 0;
                    return await cmd.ExecuteScalarAsync();
                }
            }
        }

        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public object ExecScalarProc(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    cmd.CommandTimeout = 0;
                    return cmd.ExecuteScalar();
                }
            }
        }

        public async Task<object> ExecScalarProcAsync(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    cmd.CommandTimeout = 0;
                    return await cmd.ExecuteScalarAsync();
                }
            }
        }

        /// <summary>
        /// Executes a query and returns the results as a SqlDataReader
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a SqlDataReader</returns>
        public SqlDataReader ExecDataReader(string qry, params object[] args)
        {
            Connect();         
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                return cmd.ExecuteReader();
            }
        }

        public async Task<SqlDataReader> ExecDataReaderAsync(string qry, params object[] args)
        {
            Connect();
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                return await cmd.ExecuteReaderAsync();
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the results as a SqlDataReader
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a SqlDataReader</returns>
        public SqlDataReader ExecDataReaderProc(string qry, params object[] args)
        {
            Connect();
            using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
            {
                return cmd.ExecuteReader();
            }
        }
        public async Task<SqlDataReader> ExecDataReaderProcAsync(string qry, params object[] args)
        {
            Connect();
            using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
            {
                return await cmd.ExecuteReaderAsync();
            }
        }

        /// <summary>
        /// Executes a query and returns the results as a DataSet
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataSet</returns>
        public DataSet ExecDataSet(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the results as a Data Set
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataSet</returns>
        public DataSet ExecDataSetProc(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    cmd.CommandTimeout = 0;
                    SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapt.Fill(ds);
                    return ds;
                }
            }
        }

        /// <summary>
        /// Loads the data table.
        /// </summary>
        /// <param name="command">The command.
        /// <param name="tableName">Name of the table.
        /// <returns></returns>
        public DataTable ExecDataTable(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
                {
                    cmd.CommandTimeout = 0;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns the results as a Data Table
        /// </summary>
        /// <param name="proc">Name of stored proceduret</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataSet</returns>
        public DataTable ExecDataTableProc(string qry, params object[] args)
        {
            using (SqlConnection conn = Connect())
            {
                using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
                {
                    cmd.CommandTimeout = 0;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }


        /// <summary>Opens a SQL text query or stored procedure and asynchronously reads it into a table, which is then returned.</summary>
        /// <remarks>There are several overloads, allowing for either a stored procedure or text query, with or without parameters, and
        /// with or without cancellation.</remarks>
        private static async Task<int> InternalExecuteQuery(string query, Dictionary<string, object> parameters, CommandType commandType, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var adapter = new SqlDataAdapter(query, connection))
                {
                    var table = new DataTable();
                    adapter.SelectCommand.CommandType = commandType;
                    adapter.SelectCommand.CommandTimeout = CommandTimeout;

                    foreach (var kvp in parameters)
                    {
                        adapter.SelectCommand.Parameters.AddWithValue(kvp.Key, kvp.Value);
                    }

                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    return await adapter.SelectCommand.ExecuteNonQueryAsync();
                }
            }
        }

        #endregion

        #region Transaction Members

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <returns>The new SqlTransaction object</returns>
        public SqlTransaction BeginTransaction()
        {
            Rollback();
            _trans = _conn.BeginTransaction();
            return Transaction;
        }

        /// <summary>
        /// Commits any transaction in effect.
        /// </summary>
        public void Commit()
        {
            if (_trans != null)
            {
                _trans.Commit();
                _trans = null;
            }
        }

        /// <summary>
        /// Rolls back any transaction in effect.
        /// </summary>
        public void Rollback()
        {
            if (_trans != null)
            {
                _trans.Rollback();
                _trans = null;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Need to dispose managed resources if being called manually
                if (disposing)
                {
                    if (_conn != null)
                    {
                        Rollback();
                        _conn.Dispose();
                        _conn = null;
                    }
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
