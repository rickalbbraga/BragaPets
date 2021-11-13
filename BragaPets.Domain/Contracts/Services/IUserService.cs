using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BragaPets.Domain.DTOs.Responses;
using BragaPets.Domain.DTOs.Requests;

namespace BragaPets.Domain.Contracts.Services
{
    public interface IUserService
    {
        Task<UserResponse> Add(UserRequest dto);

        Task<IEnumerable<UserResponse>> GetAll();

        Task<UserResponse> GetById(Guid uid);

        Task Delete(Guid uid);

        Task<UserResponse> Update(Guid uid, UserUpdateRequest dto);
    }
}