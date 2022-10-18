using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIs.Data;
using APIs.DTOs;
using APIs.Entities;
using APIs.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    [Authorize]

    public class UsersController : BaseApiController
    {
      
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController( IUserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        [HttpGet]
      

        public async Task<ActionResult<IEnumerable<MembersDto>>> GetUsers(){
           var users = await _userRepository.GetMembersAsync();
           var usersToReturn = _mapper.Map<IEnumerable<MembersDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{username}")]

        public async Task<ActionResult<MembersDto>> GetUser(string username){
            
            
            return  await _userRepository.GetMemberAsync(username);
           
        }
        

        
        
    }
}