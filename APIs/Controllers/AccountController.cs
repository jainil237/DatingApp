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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController( DataContext context , ITokenService tokenService)
        {
            _context= context;
            _tokenService= tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register( RegisterDto registerDto){
            if( await UserExist(registerDto.Username) ) return BadRequest( $" {registerDto.Username} is already taken "  );
            
            using var hmac  = new HMACSHA512();

            var  user = new AppUser{
                UserName= registerDto.Username,
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
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
                 };
        }

        private async Task<bool> UserExist( string username){

            return await _context.User.AnyAsync( x=> x.UserName == username.ToLower() );
        }


    }    

    
}