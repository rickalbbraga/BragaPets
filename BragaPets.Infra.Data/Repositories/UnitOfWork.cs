using System;
using System.Data;
using System.Data.SqlClient;
using BragaPets.Domain.Contracts.Repositories;

namespace BragaPets.Infra.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection dbConnection;
        private IDbTransaction dbTransaction;

        public IDbConnection DbConnection => dbConnection;
        public IDbTransaction DbTransaction => dbTransaction;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
            dbConnection = new SqlConnection(_connectionString);
        }
        
        public void Open()
        {
            dbConnection.Open();
        }

        public void Begin()
        {
            dbTransaction = dbConnection.BeginTransaction();
        }

        public void Commit()
        {
            dbTransaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            dbTransaction.Rollback();
            Dispose();
        }

        public void Close()
        {
            dbConnection.Close();
        }

        public void Dispose()
        {
            if (dbTransaction != null)
                dbTransaction.Dispose();
            dbTransaction = null;
        }
    }
}