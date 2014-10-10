using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Transactions;

namespace Bettery.Kiosk.DataAccess
{
    public abstract class SqlHelper
    {
        /// <summary>
        /// Toes the int32.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static int ToInt32(SqlDataReader reader, string columnName)
        {
            int result = 0;
            int value;
            if (reader[columnName] != DBNull.Value && int.TryParse(reader[columnName].ToString(), out value))
            {
                result = Convert.ToInt32(reader[columnName].ToString(), CultureInfo.CurrentCulture);
            }

            return result;
        }

        /// <summary>
        /// To the int32.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static int ToInt32(SqlDataReader reader)
        {
            int result = 0;
            int value;
            if (reader[0] != DBNull.Value && int.TryParse(reader[0].ToString(), out value))
            {
                result = Convert.ToInt32(reader[0].ToString(), CultureInfo.CurrentCulture);
            }

            return result;
        }

        /// <summary>
        /// Executes the dataset.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string connectionString, SqlCommand command)
        {
            DataSet dataSet = new DataSet();
            command.Connection = GetConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataSet);

            // DisposeConnection(command.Connection);
            return dataSet;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string sqlStatement)
        {
            SqlCommand command = new SqlCommand(sqlStatement);
            command.CommandType = commandType;
            command.Connection = GetConnection(connectionString);
            int result = command.ExecuteNonQuery();

            // DisposeConnection(command.Connection);
            return result;
        }

        /// <summary>
        /// Gets the transaction option.
        /// </summary>
        /// <returns></returns>
        public static TransactionOptions GetTransactionOption()
        {
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.Serializable;

            TimeSpan transactionTimeOut;
            bool result = TimeSpan.TryParse(ConfigurationManager.AppSettings["transactionTimeOut"], out transactionTimeOut);

            if (result)
            {
                transactionOptions.Timeout = transactionTimeOut;
            }

            return transactionOptions;
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        internal static SqlDataReader ExecuteReader(string connectionString, SqlCommand command)
        {
            command.Connection = GetConnection(connectionString);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns></returns>
        internal static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string sqlStatement)
        {
            SqlCommand command = new SqlCommand(sqlStatement);
            command.CommandType = commandType;
            command.Connection = GetConnection(connectionString);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        internal static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string sqlStatement, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(sqlStatement);
            command.CommandType = commandType;
            command.Connection = GetConnection(connectionString);
            PopulateSqlParameters(command, parameters);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        internal static SqlDataReader ExecuteReader(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcedureName)
            {
                CommandType = CommandType.StoredProcedure,
                Connection = GetConnection(connectionString)
            };

            PopulateSqlParameters(command, parameters);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Executes the dataset.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        internal static DataSet ExecuteDataset(string connectionString, CommandType commandType, string sqlStatement, params SqlParameter[] parameters)
        {
            SqlConnection connection = GetConnection(connectionString);
            SqlCommand command = new SqlCommand(sqlStatement) { CommandType = commandType };
            PopulateSqlParameters(command, parameters);
            command.Connection = connection;
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            // DisposeConnection(connection);
            return dataSet;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        internal static int ExecuteNonQuery(string connectionString, SqlCommand command)
        {
            command.Connection = GetConnection(connectionString);
            int result = command.ExecuteNonQuery();

            // DisposeConnection(command.Connection);
            return result;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="storedProcedure">The stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        internal static int ExecuteNonQuery(string connectionString, string storedProcedure, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcedure);
            command.CommandType = CommandType.StoredProcedure;
            PopulateSqlParameters(command, parameters);
            command.Connection = GetConnection(connectionString);
            int result = command.ExecuteNonQuery();

            // DisposeConnection(command.Connection);
            return result;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        internal static int ExecuteNonQuery(string connectionString, CommandType commandType, string sqlStatement, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(sqlStatement);
            command.CommandType = commandType;
            command.Connection = GetConnection(connectionString);
            PopulateSqlParameters(command, parameters);
            int result = command.ExecuteNonQuery();

            // DisposeConnection(command.Connection);
            return result;
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns></returns>
        internal static object ExecuteScalar(string connectionString, CommandType commandType, string sqlStatement)
        {
            SqlCommand command = new SqlCommand(sqlStatement);
            command.CommandType = commandType;
            command.Connection = GetConnection(connectionString);
            object result = command.ExecuteScalar();

            // DisposeConnection(command.Connection);
            return result;
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        internal static object ExecuteScalar(string connectionString, SqlCommand command)
        {
            // Case#11 Create function Exec command check user login
            command.Connection = GetConnection(connectionString);
            object result = command.ExecuteScalar();

            // DisposeConnection(command.Connection);
            return result;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        private static SqlConnection GetConnection(string connectionString)
        {
            SqlConnection connection;
            if (DbConnectionScope.Current == null)
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            else
            {
                connection = (SqlConnection)DbConnectionScope.Current.GetOpenConnection(SqlClientFactory.Instance, connectionString);
            }

            return connection;
        }

        /// <summary>
        /// Disposes the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        private static void DisposeConnection(SqlConnection connection)
        {
            if (DbConnectionScope.Current == null)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Populates the SQL parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        private static void PopulateSqlParameters(SqlCommand command, params SqlParameter[] parameters)
        {
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    command.Parameters.Add(parameter);
                }
            }
        }
    }
}