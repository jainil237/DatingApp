using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIs.Entities;
using APIs.DTOs;
using APIs.Helpers;

namespace APIs.Interfaces{
    public interface IUserRepository
    {
               void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<PagedList<MembersDto>> GetMembersAsync(UserParams userParams);
        Task<MembersDto> GetMemberAsync(string username);
    }

}
