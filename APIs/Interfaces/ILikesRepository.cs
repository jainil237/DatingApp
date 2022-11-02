using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIs.DTOs;
using APIs.Entities;
using APIs.Helpers;

namespace APIs.Interfaces
{
    public interface ILikesRepository
    {
         Task<UserLikes> GetUserLike(int sourceUserId, int likedUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}