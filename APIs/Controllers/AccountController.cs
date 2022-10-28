using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using APIs.Data;
using APIs.DTOs;
using APIs.Entities;
using APIs.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController( DataContext context , ITokenService tokenService , IMapper mapper)
        {
            _context= context;
            _tokenService= tokenService;
            _mapper= mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register( RegisterDto registerDto){
            if( await UserExist(registerDto.Username) ) return BadRequest( $" {registerDto.Username} is already taken "  );
            var user = _mapper.Map<AppUser>(registerDto);
            using var hmac  = new HMACSHA512();

            
                user.UserName= registerDto.Username.ToLower();
                user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
                user.PasswordSalt = hmac.Key;
           

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs= user.KnownAs,
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user = await  _context.User.
                         Include(p=>p.Photos)
                        .SingleOrDefaultAsync( x => x.UserName == loginDto.Username.ToLower());  // we could have used FirstOrDefaultAsync() insted but It couldn't handle like the latter

                if(user == null) return Unauthorized("Invalid  Username ");
                 
                 var hmac = new HMACSHA512(user.PasswordSalt);

                 var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

                 for(int i=0 ; i<ComputedHash.Length;i++) {
                    if ( ComputedHash[i] != user.PasswordHash[i]) return Unauthorized(" Invalid Password");

                 }
                 return new UserDto{
                    Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                 };
        }

        private async Task<bool> UserExist( string username){

            return await _context.User.AnyAsync( x=> x.UserName == username.ToLower() );
        }


    }    

    
}