using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BragaPets.Domain.Entities;

namespace BragaPets.Domain.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user);

        Task<IEnumerable<User>> GetAll();

        Task<User> GetById(Guid uid);

        Task Delete(Guid uid);

        Task Update(User user);
    }
}