using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIs.DTOs;
using APIs.Entities;
using APIs.Helpers;
using APIs.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace APIs.Data
{
    public class UserRepository : IUserRepository
    {
   private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MembersDto> GetMemberAsync(string username)
        {
            return await _context.User
                .Where(x => x.UserName == username)
                .ProjectTo<MembersDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        // public async Task<IEnumerable<MembersDto>> GetMembersAsync(UserParams userParams)
        // {
        //     // var query=_context.User.ProjectTo<MembersDto>(_mapper.ConfigurationProvider).AsNoTracking();
        //     // return await PagedList<MembersDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);



        //     return await _context.User
        //         .ProjectTo<MembersDto>(_mapper.ConfigurationProvider)
        //         .ToListAsync();
        // }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.User
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.User
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; 
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        async   Task<PagedList<MembersDto>> IUserRepository.GetMembersAsync(UserParams userParams)
        {
            
            var query=_context.User.AsQueryable();
                        
                query = query.Where(u=>u.UserName != userParams.CurrentUsername);  
                query = query.Where( u=> u.Gender == userParams.Gender);     
                var minDob= DateTime.Today.AddYears(-userParams.MaxAge -1);
                var maxDob= DateTime.Today.AddYears(-userParams.MinAge);
                query = query.Where(u=>u.DateOfBirth >= minDob && u.DateOfBirth <=maxDob);
                query = userParams.OrderBy switch{
                  "created"=> query.OrderByDescending(u=>u.Created),
                  _=> query.OrderByDescending(u=>u.LastActive)  
                    
                };
                
            return await PagedList<MembersDto>.CreateAsync(query.ProjectTo<MembersDto>(_mapper
            .ConfigurationProvider).AsNoTracking(), 
            userParams.PageNumber, userParams.PageSize);
        }
    }
}