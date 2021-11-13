using System.Data;

namespace BragaPets.Domain.Contracts.Repositories
{
    public interface IUnitOfWork
    {
        IDbConnection DbConnection { get; }
        IDbTransaction DbTransaction { get; }

        void Open();
        void Begin();
        void Commit();
        void Rollback();
        void Close();
    }
}