using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BragaPets.Domain.Contracts.Repositories;
using BragaPets.Domain.Entities;
using BragaPets.Domain.Enums;

namespace BragaPets.Infra.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task Add(User user)
        {
            var query = @"
                    INSERT INTO USERS(
                        Uid
                        ,Name
                        ,Email
                        ,Type
                        ,Status
                        ,FirstAccess
                        ,CreateDate
                    )VALUES(
                        @Uid
                        ,@Name
                        ,@Email
                        ,@Type
                        ,@Status
                        ,@FirstAccess
                        ,GETDATE())";

            var param = new
            {
                user.Uid,
                user.Name,
                user.Email,
                Type = Enum.GetName(typeof(UserType), user.Type),
                Status = Enum.GetName(typeof(UserStatus), user.Status),
                user.FirstAccess
            };

            await base.Add(query, param);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var query = @"
                SELECT 
                    Uid
                    ,Name
                    ,Email
                    ,Type
                    ,Status
                    ,FirstAccess
                    ,CreateDate
                FROM USERS
                WHERE
                    STATUS = 'ACTIVE'";

            return await base.GetAll(query);
        }

        public async Task<User> GetById(Guid uid)
        {
            var query = @"
                SELECT 
                    Uid
                    ,Name
                    ,Email
                    ,Type
                    ,Status
                    ,FirstAccess
                    ,CreateDate
                FROM USERS
                WHERE UID = @uid
                    AND STATUS = 1";

            var param = new { uid };

            return await base.GetById(query, param);
        }

        public async Task Delete(Guid uid)
        {
            var query = @"
                UPDATE USERS 
                SET 
                    Status = @Status
                    ,DeleteDate = GETDATE()
                WHERE Uid = @uid
            ";

            var param = new { @Status = UserStatus.INACTIVE, uid };

            await base.Delete(query, param);
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}