using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIs.Entities;
using APIs.DTOs;
namespace APIs.Interfaces;
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool>SaveAllAsync();
        Task<IEnumerable<AppUser>>GetUsersAsync();
        Task<AppUser>GetUserByIdAsync(int Id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<IEnumerable<MembersDto>>GetMembersAsync();
        Task<MembersDto>GetMemberAsync(string username);


     }
