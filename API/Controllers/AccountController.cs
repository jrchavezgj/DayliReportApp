using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        public DataContext _context ;
        public  AccountController(DataContext context) {
            _context = context;

         }

         [HttpPost("register")]
         public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto){
             
             using var hmac = new HMACSHA512();
             var user = new AppUser{
                 UserName = registerDto.Username,
                 PassordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                 PasswordSalt = hmac.Key
             };
             _context.Users.Add(user);
             await _context.SaveChangesAsync();
             return user;
         }

         private async Task<bool> UserExist(string username){
             return await _context.Users.AnyAsync(x = x.UserName == username.ToLower());
         }
    }
}