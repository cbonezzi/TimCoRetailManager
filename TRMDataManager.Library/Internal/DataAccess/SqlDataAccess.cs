using Dapper;
using Microsoft.Extensions.Configuration;
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
		public SqlDataAccess(IConfiguration config)
		{
			_config = config;
		}

		public string GetConnectionString(string name)
		{
			return _config.GetConnectionString(name);
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

			isClosed = false;

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

		private bool isClosed = false;
		private readonly IConfiguration _config;

		public void CommitTransaction()
		{
			_transaction?.Commit();
			_connection?.Close();

			isClosed = true;
		}

		public void RollbackTransaction()
		{
			_transaction?.Rollback();
			_connection?.Close();
			
			isClosed = true;
		}

		public void Dispose()
		{
			if (isClosed == false)
			{
				try
				{
					CommitTransaction();
				}
				catch (Exception)
				{

					// TODO - log this issue
				}
			}

			_transaction = null;
			_connection = null;
		}

		// Open connection/start transaction method
		// load using the transaction 
		// save using the transaction
		// close connection/stop transaction method
		// need to implement dispose - which will help clean up the about connection/transaction

	}
}
