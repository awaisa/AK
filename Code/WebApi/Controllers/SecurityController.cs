using BusinessCore.Data;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using BusinessCore.Services.Security;
using WebApiCore.Models;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApiCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApiCore.Infrastructure.ErrorHandling;
using BusinessCore.Security;

namespace WebApiCore.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [EnableCors("CorsPolicy")]
    public class SecurityController : Controller
    {
        private ISecurityService accountRepo;
        private readonly AppSettings _appSettings;

        public SecurityController(ISecurityService actRepo, IOptions<AppSettings> appSettings)
        {
            accountRepo = actRepo;
            _appSettings = appSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/login")]
        public IActionResult Login([FromBody]  UserInModel loginUser)
        {
            var user = accountRepo.AuthenticateAndLoadUser(loginUser.Username, loginUser.Password);

            if (user == null)
                throw new ApiException("Invalid Login Credentials", 401);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(AppClaims.UserId, user.Id.ToString()),
                    new Claim(AppClaims.Username, user.Username),
                    new Claim(AppClaims.Firstname, user.Firstname),
                    new Claim(AppClaims.Lastname, user.Lastname),
                    new Claim(AppClaims.CompanyId, user.CompanyId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new UserOutModel
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Token = tokenString
            });
        }

        /// <summary>
        /// We are using JWT Bearer signIn so to logout simply remove the token from client.
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("api/logout")]
        //public async Task<bool> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);            
        //    return true;
        //}

        [HttpGet]
        [Route("api/isAuthenticated")]
        public bool IsAuthenthenticated()
        {
            return User.Identity.IsAuthenticated;
        }
    }
}
