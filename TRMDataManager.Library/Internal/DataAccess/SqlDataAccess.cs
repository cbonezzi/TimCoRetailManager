using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDataManager.Library.Internal.DataAccess
{
	internal class SqlDataAccess : IDisposable
	{
		public string GetConnectionString(string name)
		{
			return ConfigurationManager.ConnectionStrings[name].ConnectionString;
		}

		public List<T> LoadData<T, U>(string storeProcedure, U parameters, string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);

			using (IDbConnection cnn = new SqlConnection(connectionString))
			{
				List<T> rows = cnn.Query<T>(storeProcedure, parameters, commandType: CommandType.StoredProcedure).ToList();
				return rows;
			}
		}

		public void SaveData<T>(string storeProcedure, T parameters, string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);

			using (IDbConnection cnn = new SqlConnection(connectionString))
			{
				cnn.Execute(storeProcedure, parameters, commandType: CommandType.StoredProcedure);
			}
		}

		private IDbConnection _connection;
		private IDbTransaction _transaction;

		public void StartTransaction(string connectionStringName)
		{
			string connectionString = GetConnectionString(connectionStringName);
			
			_connection = new SqlConnection(connectionString);
			
			_connection.Open();

			_transaction = _connection.BeginTransaction();


		}

		public List<T> LoadDataInTransaction<T, U>(string storeProcedure, U parameters)
		{
			List<T> rows = _connection.Query<T>(storeProcedure, parameters,
				commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
			return rows;
		}

		public void SaveDataInTransaction<T>(string storeProcedure, T parameters)
		{
			_connection.Execute(storeProcedure, parameters,
				commandType: CommandType.StoredProcedure, transaction: _transaction);
		}
		public void CommitTransaction()
		{
			_transaction?.Commit();
			_connection?.Close();
		}

		public void RollbackTransaction()
		{
			_transaction?.Rollback();
			_connection?.Close();
		}

		public void Dispose()
		{
			CommitTransaction();
		}

		// Open connection/start transaction method
		// load using the transaction 
		// save using the transaction
		// close connection/stop transaction method
		// need to implement dispose - which will help clean up the about connection/transaction

	}
}
