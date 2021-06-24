using Dev.Core.Domain.Users;
using Dev.Plugin.Sys.Auth.Configuration;
using Dev.Plugin.Sys.Auth.Models;
using Dev.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Plugin.Sys.Auth.Controllers
{
    public class AuthenticateController : PublicController
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IUserService _userService;

        public AuthenticateController(JwtConfig jwtConfig,
                                      IUserService userService)
        {
            _jwtConfig = jwtConfig;
            _userService = userService;
        }

        #region Utilities
        private string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
        #endregion


        #region Rest APIs

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel user)
        {
            if (ModelState.IsValid)
            {
                // We can utilise the model
                User existingUser = await _userService.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponseModel()
                    {
                        Errors = new List<string>() {
                                "Email already in use"
                            },
                        Success = false
                    });
                }

                UserRegistrationRequest userRegistration = new UserRegistrationRequest()
                {
                    Email = user.Email,
                    Username = user.Username,
                    Password = user.Password,
                    PasswordFormat = PasswordFormat.Hashed,
                    IsApproved = true,
                    WebAppId = Guid.Empty
                };

                var registrationResult = await _userService.RegisterUserAsync(userRegistration);
                User newUser = new User()
                {
                    Email = user.Email,
                    UserName = user.Username
                };


                var jwtToken = GenerateJwtToken(newUser);

                return Ok(new RegistrationResponseModel()
                {
                    Success = true,
                    Token = jwtToken
                });
            }

            return BadRequest(new RegistrationResponseModel()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        #endregion
    }
}
