using Dev.Core.Domain.Users;
using Dev.Plugin.Sys.Auth.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Dev.Plugin.Sys.Auth.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetByEmail(string email);
        User GetById(Guid userId);
    }


    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Email = "chitmeo@gmail.com", Active = true, Deleted = false, Id = Guid.Parse("fd7e86d4-d0a6-47f6-a32d-7bd175968a32") },
            new User { Email = "admin@test.com", Active = true, Deleted = false, Id = Guid.Parse("36bfef78-664f-4e71-9539-3f258a129d7c") }
        };

        public UserService()
        {

        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Email == model.Email && model.Password == "MotHaiBa!@#");

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetByEmail(string email)
        {
            return _users.FirstOrDefault(x => x.Email == email);
        }

        public User GetById(Guid userId)
        {
            return _users.FirstOrDefault(x => x.Id == userId);
        }

        // helper methods

        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("SecurityCodeNoShareOK");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
