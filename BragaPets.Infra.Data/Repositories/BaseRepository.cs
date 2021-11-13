using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BragaPets.Domain.Contracts.Repositories;
using BragaPets.Domain.Entities;
using Dapper;

namespace BragaPets.Infra.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        private readonly IUnitOfWork _uow;

        protected BaseRepository(IUnitOfWork uow)
        {
            _uow = uow;
        }
        
        public async Task Add(string query, object param)
        {
            try
            {
                _uow.Open();
                _uow.Begin();
                await _uow.DbConnection.ExecuteAsync(query, param, _uow.DbTransaction);
                _uow.Commit();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
            finally
            {
                _uow.Close();
            }
        }

        public async Task Delete(string query, object param)
        {
            try
            {
                _uow.Open();
                _uow.Begin();
                await _uow.DbConnection.ExecuteAsync(query, param, _uow.DbTransaction);
                _uow.Commit();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
            finally
            {
                _uow.Close();
            }
        }

        public async Task<T> GetById(string query, object param)
        {
            try
            {
                _uow.Open();
                return await _uow.DbConnection.QueryFirstOrDefaultAsync<T>(query, param);
            }
            catch
            {
                throw;
            }
            finally
            {
                _uow.Close();
            };
        }

        public async Task<IEnumerable<T>> GetAll(string query)
        {
            try
            {
                _uow.Open();
                return await _uow.DbConnection.QueryAsync<T>(query);
            }
            catch
            {
                throw;
            }
            finally 
            {
                _uow.Close();
            };
        }

        public async Task Update(string query, object param)
        {
            try
            {
                _uow.Open();
                _uow.Begin();
                await _uow.DbConnection.ExecuteAsync(query, param, _uow.DbTransaction);
                _uow.Commit();
            }
            catch
            {
                _uow.Rollback();
                throw;
            }
            finally
            {
                _uow.Close();
            }
        }
    }
}