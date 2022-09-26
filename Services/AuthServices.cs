#nullable disable
using System;
using Microsoft.AspNetCore.Identity;
using Clinic.Models;
using ClinicApi.Interfaces;
using ClinicApi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using Clinic.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ClinicApi.Models;
using Microsoft.EntityFrameworkCore;
using ClinicApi.Data.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace ClinicApi.Services
{
    public class AuthServices : IAuth
    {
        

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ClinicContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IMemoryCache _cache;
        public AuthServices(ClinicContext context, 
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters,
            IMemoryCache cache)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
            _cache = cache;
        }
        
        //Login Functionality Implmented
        public async Task<AuthTokenVM> Login([FromBody] LoginVM loginVM)
        {

            var userExists = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (userExists != null && await _userManager.CheckPasswordAsync(userExists,loginVM.Password))
            {
                var tokenValue = await GenerateJWTTokenAsync(userExists, null);
                return tokenValue;
            }
            return null;
        } 
        public async Task<AuthTokenVM> RefreshToken([FromBody] RefreshTokenVM refreshTokenVM)
        {
            var result = await VerifyAndGenerateTokenAsync(refreshTokenVM);
            return result;
        }

        private async Task<AuthTokenVM> VerifyAndGenerateTokenAsync(RefreshTokenVM refreshTokenVM)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            AuthTokenVM authToken = new AuthTokenVM();
           User user = await _userManager.FindByIdAsync(refreshTokenVM.user_Id);
            //getting the cache value
            authToken = _cache.Get<AuthTokenVM>(key: "token");
           if(authToken == null)
            {
                return await GenerateJWTTokenAsync(user, refreshTokenVM.RefreshToken);
            }
            var storedToken = authToken.RefreshToken;
            var dbUser = await _userManager.FindByIdAsync(authToken.UserId);
            try
            {
                var tokenCheckResult = jwtTokenHandler.ValidateToken(authToken.Token, _tokenValidationParameters,
                    out var validatedToken);
                return await GenerateJWTTokenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException) 
            {
                
                   return await GenerateJWTTokenAsync(dbUser, null);

                
            }
        }

        private async Task<AuthTokenVM> GenerateJWTTokenAsync(User user, string rToken)
        {
            var authClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.Name, user.UserName),
                new Claim (ClaimTypes.NameIdentifier, user.Id),
                new Claim (JwtRegisteredClaimNames.Email,user.EmailAddress),
                new Claim (JwtRegisteredClaimNames.Sub,user.EmailAddress),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            //Add user role claims
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT.Audience"],
                expires: DateTime.UtcNow.AddMinutes(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            if (rToken != null)
            {
                var rTokenResponse = new AuthTokenVM()
                {
                    Token = jwtToken,
                    RefreshToken = rToken,
                    UserId = user.Id,
                    ExpiresAt = token.ValidTo,
                };
                _cache.Set(key: "token", rTokenResponse, TimeSpan.FromMinutes(1));
                return rTokenResponse;
            }
            var refreshToken = new RefreshToken()
            {
                JWTId = token.Id,
                IsRevoked=false,
                UserId=user.Id,
                DateAdded=DateTime.UtcNow,
                DateExpire=(DateTime.UtcNow.AddSeconds(6)).ToOADate(),
                Token=Guid.NewGuid().ToString()+"-"+Guid.NewGuid().ToString(),
            };
            
           
           // await _context.RefreshTokens.AddAsync(refreshToken);
            //await _context.SaveChangesAsync();
            var response = new AuthTokenVM()
            {
                Token = jwtToken,
                RefreshToken=refreshToken.Token,
                UserId=refreshToken.UserId,
                ExpiresAt = token.ValidTo,
            };

            //caching the token
            _cache.Set(key:"token", response, TimeSpan.FromMinutes(1) );
            return response;
        }
        //        private async Task<AuthTokenVM> GenerateJWTTokenAsync(User user, RefreshToken rToken)
        //{
        //    var authClaims = new List<Claim>()
        //    {
        //        new Claim (ClaimTypes.Name, user.UserName),
        //        new Claim (ClaimTypes.NameIdentifier, user.Id),
        //        new Claim (JwtRegisteredClaimNames.Email,user.EmailAddress),
        //        new Claim (JwtRegisteredClaimNames.Sub,user.EmailAddress),
        //        new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        //    };

        //    //Add user role claims
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    foreach(var userRole in userRoles)
        //    {
        //        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        //    }

        //    var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["JWT:Issuer"],
        //        audience: _configuration["JWT.Audience"],
        //        expires: DateTime.UtcNow.AddMinutes(1),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
        //        );
        //    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        //    if(rToken != null)
        //    {
        //        var rTokenResponse = new AuthTokenVM()
        //        {
        //            Token = jwtToken,
        //            RefreshToken = rToken.Token,
        //            ExpiresAt = token.ValidTo,
        //        };
        //        return rTokenResponse;
        //    }
        //    var refreshToken = new RefreshToken()
        //    {
        //        JWTId = token.Id,
        //        IsRevoked=false,
        //        UserId=user.Id,
        //        DateAdded=DateTime.UtcNow,
        //        DateExpire=(DateTime.UtcNow.AddSeconds(6)).ToOADate(),
        //        Token=Guid.NewGuid().ToString()+"-"+Guid.NewGuid().ToString(),
        //    };

        //    var options = new MemoryCacheEntryOptions()
        //                .SetAbsoluteExpiration(
        //                      TimeSpan.
                              
        //                      FromSeconds(Convert.ToDouble(refreshToken.DateExpire)));
        //    _cache.Set("TOKEN", refreshToken.Token, options);
        //   // await _context.RefreshTokens.AddAsync(refreshToken);
        //    //await _context.SaveChangesAsync();
        //    var response = new AuthTokenVM()
        //    {
        //        Token = jwtToken,
        //        RefreshToken=refreshToken.Token,
        //        ExpiresAt = token.ValidTo,
        //    };
        //    return response;
        //}

        public async Task<User> Register([FromBody]RegisterVM registerVM)
        {
            var userExists = await _userManager.FindByEmailAsync(registerVM.Email);
            if (userExists != null)
            {
                return null;
            }
            User newUser = new User()
            {
               UserName=registerVM.UserName,
                Name = registerVM.Name,
                Email=registerVM.Email,
                EmailAddress = registerVM.Email,
                Password = registerVM.Password,
                RoleId = registerVM.RoleId,
                SecurityStamp = Guid.NewGuid().ToString(),
                Custom=registerVM.Custom,
            };
            //var res = await _context.Users.AddAsync(newUser);
            //_context.SaveChanges();
            var result = await _userManager.CreateAsync(newUser, registerVM.Password);
            //_context.SaveChanges();
            if (result.Succeeded)
            {
                switch (registerVM.Role)
                {
                    case UserRoles.Doctor:
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Doctor);
                        break;

                    case UserRoles.Admin:
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
                        break;

                    case UserRoles.Patient:
                        await _userManager.AddToRoleAsync(newUser, UserRoles.Patient);
                        break;

                    default:
                        break;

                }
                
                return newUser;
            }
            return null;

        }
    }
}
