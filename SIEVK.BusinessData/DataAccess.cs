using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIEVK.BusinessData
{

    /// <summary>
    /// DB Access Con
    /// </summary>
    /// 

    public class DataAccess : IDisposable
    {
        private SqlConnection _sqlConnection;
        SqlTransaction _transaction = null;
        private int RTO = 120;
        private List<SqlParameter> _sqlParameters = new List<SqlParameter>();
        private List<SqlParameter> _OutputSqlParameters = new List<SqlParameter>();
        private List<object> ObjOutput = new List<object>();

        public void AddParameter(string name, object value)
        {
            name = name.Trim();
            name = (name.Substring(0, 1).Contains("@") ? name : "@" + name);
            SqlParameter sqlParameter = new SqlParameter(name, value);
            sqlParameter.Direction = ParameterDirection.Input;
            _sqlParameters.Add(sqlParameter);

        }

        public void AddParameter(string name, SqlDbType dataType, object value, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            name = name.Trim();
            name = (name.Substring(0, 1).Contains("@") ? name : "@" + name);
            SqlParameter sqlParameter = new SqlParameter(name, dataType);
            sqlParameter.Value = value;
            sqlParameter.Direction = parameterDirection;
            _sqlParameters.Add(sqlParameter);

        }

        public void AddParameter(string name, SqlDbType dataType, int size, object value)
        {
            name = name.Trim();
            name = (name.Substring(0, 1).Contains("@") ? name : "@" + name);
            SqlParameter sqlParameter = new SqlParameter(name, dataType);
            sqlParameter.Value = value;
            sqlParameter.Direction = ParameterDirection.Input;
            sqlParameter.Size = size;
            _sqlParameters.Add(sqlParameter);

        }


        public List<object> GetParamterOutput()
        {

            return ObjOutput;

        }
        public void ClearParameter()
        {
            _sqlParameters.Clear();
        }

        private void ClearParamterOutput()
        {
            _OutputSqlParameters.Clear();
            ObjOutput = new List<object>();
        }

        private void AddParameterOutput(SqlParameter sqlParameter)
        {
            if (sqlParameter.Direction == ParameterDirection.Output || sqlParameter.Direction == ParameterDirection.Output)
            {
                _OutputSqlParameters.Add(sqlParameter);
                ObjOutput.Add(sqlParameter.Value);
            }
        }

        /// <summary>
        /// String Connection
        /// </summary>
        /// <param name="connectionID">Name of your String Connection</param>
        public DataAccess(string connectionID)
        {
            string constr = ConfigurationManager.ConnectionStrings[connectionID.Trim()].ConnectionString;
            _sqlConnection = new SqlConnection(constr);
            _sqlConnection.Open();
        }


        /// <summary>
        /// Default String Connn
        /// </summary>
        public DataAccess()
        {

            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings
                    ["SIEVKConnection"].ConnectionString);
            _sqlConnection.Open();
        }

        public void BeginTransaction()
        {
            _transaction = _sqlConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }

        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        public void ExecuteNonQuery(string storedProcedureName, CommandType CommandTYpe = CommandType.StoredProcedure)
        {
            using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, _sqlConnection))
            {
                try
                {

                    sqlCommand.CommandType = CommandTYpe;
                    sqlCommand.CommandTimeout = 0;
                    ClearParamterOutput();
                    foreach (SqlParameter sqlParameter in _sqlParameters)
                    {
                        AddParameterOutput(sqlParameter);
                        sqlCommand.Parameters.Add(sqlParameter);
                    }
                    sqlCommand.ExecuteNonQuery();
                    // return sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                    _sqlParameters.Clear();
                }
            }
        }

        public void ExecuteNonQueryAsync(string storedProcedureName, CommandType CommandTYpe = CommandType.StoredProcedure)
        {
            using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, _sqlConnection))
            {
                try
                {

                    sqlCommand.CommandType = CommandTYpe;
                    sqlCommand.CommandTimeout = 0;
                    ClearParamterOutput();
                    foreach (SqlParameter sqlParameter in _sqlParameters)
                    {
                        AddParameterOutput(sqlParameter);
                        sqlCommand.Parameters.Add(sqlParameter);
                    }
                    sqlCommand.ExecuteNonQueryAsync();
                    // return sqlCommand.ExecuteNonQuery();
                }
                finally
                {
                    _sqlParameters.Clear();
                }
            }
        }

        public object ExecuteScalar(string storedProcedureName, CommandType CommandTYpe = CommandType.StoredProcedure)
        {
            using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, _sqlConnection))
            {
                try
                {
                    sqlCommand.CommandType = CommandTYpe;
                    sqlCommand.CommandTimeout = 0;
                    ClearParamterOutput();
                    foreach (SqlParameter sqlParameter in _sqlParameters)
                    {
                        AddParameterOutput(sqlParameter);
                        sqlCommand.Parameters.Add(sqlParameter);
                    }
                    return sqlCommand.ExecuteScalar();
                }
                finally
                {
                    _sqlParameters.Clear();
                }
            }
        }

        public SqlDataReader ExecuteDataReader(string storedProcedureName, CommandType CommandTYpe = CommandType.StoredProcedure)
        {
            using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, _sqlConnection))
            {
                try
                {
                    sqlCommand.CommandType = CommandTYpe;
                    sqlCommand.CommandTimeout = 0;
                    ClearParamterOutput();
                    foreach (SqlParameter sqlParameter in _sqlParameters)
                    {
                        AddParameterOutput(sqlParameter);
                        sqlCommand.Parameters.Add(sqlParameter);
                    }
                    return sqlCommand.ExecuteReader();
                }
                finally
                {
                    _sqlParameters.Clear();
                }
            }
        }

        public DataTable ExecuteDataTable(string storedProcedureName, CommandType CommandTYpe = CommandType.StoredProcedure)
        {
            using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, _sqlConnection))
            {
                try
                {
                    sqlCommand.CommandType = CommandTYpe;
                    sqlCommand.CommandTimeout = 0;
                    ClearParamterOutput();
                    foreach (SqlParameter sqlParameter in _sqlParameters)
                    {
                        AddParameterOutput(sqlParameter);
                        sqlCommand.Parameters.Add(sqlParameter);
                    }
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataSet);
                    DataTable dataTable = dataSet.Tables[0];
                    dataSet.Tables.Remove(dataTable);
                    return dataTable;
                }
                finally
                {
                    _sqlParameters.Clear();
                }
            }
        }

        public DataSet ExecuteDataSet(string storedProcedureName)
        {
            using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName, _sqlConnection))
            {
                try
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandTimeout = 0;
                    ClearParamterOutput();
                    foreach (SqlParameter sqlParameter in _sqlParameters)
                    {
                        AddParameterOutput(sqlParameter);
                        sqlCommand.Parameters.Add(sqlParameter);
                    }
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataSet);
                    return dataSet;
                }
                finally
                {
                    _sqlParameters.Clear();
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_sqlConnection != null)
            {
                _sqlConnection.Close();
                _sqlConnection.Dispose();
                _sqlConnection = null;
            }
        }

        #endregion
    }
}