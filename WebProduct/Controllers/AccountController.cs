using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Api_Store.Data;
using Api_Store.Dtos;
using Api_Store.Entity;
using Api_Store.Interfaces;



namespace Api_Store.Controllers
    {
    public class AccountController(DataContext _context, ITokenService _tokenService) : ControllerBase
        {
        [HttpPost("/account")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
            {
            if (string.IsNullOrWhiteSpace(registerDto.Email))
                {
                return BadRequest("L'adresse e-mail est requise.");
                }
            if (await UserExists(registerDto.Email))
                {
                return BadRequest("L'adresse e-mail est déjà utilisée.");
                }
            if (string.IsNullOrWhiteSpace(registerDto.Password))
                {
                return BadRequest("Le mot de passe est requis.");
                }

            using var hmac = new HMACSHA512();

            var user = new User
                {
                Email = registerDto.Email.ToLower(),
                Firstname= registerDto.Firstname,
                Username= registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
                };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
                {
                Firstname= user.Firstname,
               Username=user.Username,

                
                Email = user.Email,
                Token = _tokenService.CreateToken(user)

                };


            }

        [HttpPost("/token")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
            {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null) return Unauthorized("Invalide Email");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computHash.Length; i++)
                {
                if (computHash[i] != user.PasswordHash[i]) return Unauthorized("Invalide password");
                }
            return new UserDto
                {
                Email = user.Email,
                Firstname = user.Firstname,
                Username = user.Username,
                Token = _tokenService.CreateToken(user)

                };
           
            }

        private async Task<bool> UserExists(string email)
            {

            string normalizedEmail = email.ToLower();
            return await _context.User.AnyAsync(x => x.Email == normalizedEmail);

            }



        }
    }
    
