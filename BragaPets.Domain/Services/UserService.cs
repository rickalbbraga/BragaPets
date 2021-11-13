using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BragaPets.Domain.Contracts.Repositories;
using BragaPets.Domain.DTOs.Responses;
using BragaPets.Domain.Contracts.Services;
using BragaPets.Domain.DTOs.Requests;
using BragaPets.Domain.Entities;
using BragaPets.Domain.Notifications;

namespace BragaPets.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly NotificationContext _notificationContext;
        private readonly IMapper _mapper;


        public UserService(
            IUserRepository userRepository,
            NotificationContext notificationContext,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _notificationContext = notificationContext;
            _mapper = mapper;
        }
        
        public async Task<UserResponse> Add(UserRequest dto)
        {
            var user = User.Create(dto);
            if (user.HasErrors())
            {
                _notificationContext.AddNotification(
                    user.Errors.Select(e => new Notification(e.ErrorCode, e.ErrorMessage)).ToList());

                return null;
            }
            
            await _userRepository.Add(user);

            return _mapper.Map<User, UserResponse>(user);
        }

        public async Task<IEnumerable<UserResponse>> GetAll()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse> GetById(Guid uid)
        {
            ValidateAnUid(uid);

            if (_notificationContext.HasNotifications) return null;
            
            var user = await _userRepository.GetById(uid);
            
            return _mapper.Map<User, UserResponse>(user);
        }

        private void ValidateAnUid(Guid uid)
        {
            if (uid == Guid.Empty)
                _notificationContext.AddNotification(string.Empty, "Invalid uid");
        }

        public async Task Delete(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                _notificationContext.AddNotification(string.Empty, "Invalid uid");
                return;
            }
            
            await _userRepository.Delete(uid);
        }

        public async Task<UserResponse> Update(Guid uid, UserUpdateRequest dto)
        {
            if (uid == Guid.Empty)
            {
                _notificationContext.AddNotification(string.Empty, "Invalid uid");
                return null;
            }

            var foundUser = await _userRepository.GetById(uid);
            if (foundUser is null)
            {
                _notificationContext.AddNotification(string.Empty, "Invalid uid");
                return null;
            }

            await _userRepository.Delete(uid);
            
            foundUser.Update(dto);
            if (foundUser.HasErrors())
            {
                _notificationContext.AddNotification(
                    foundUser.Errors.Select(e => new Notification(e.ErrorCode, e.ErrorMessage)).ToList());
            
                return null;
            }

            await _userRepository.Add(foundUser);

            return _mapper.Map<User, UserResponse>(foundUser);
        }
    }
}