using System.Collections.Generic;
using System.Threading.Tasks;
using BragaPets.Domain.Entities;

namespace BragaPets.Domain.Contracts.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task Add(string query, object param);

        Task Delete(string query, object param);

        Task<T> GetById(string query, object param);

        Task<IEnumerable<T>> GetAll(string query);

        Task Update(string query, object param);
    }
}