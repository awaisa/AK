using BusinessCore.Data;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using BusinessCore.Services.Security;
using WebApiCore.Models;

namespace AlbumViewerAspNetCore
{    
    [ServiceFilter(typeof(ApiExceptionFilter))]    
    [EnableCors("CorsPolicy")]
    public class SecurityController : Controller
    {
        private ISecurityService accountRepo;

        public SecurityController(ISecurityService actRepo)            
        {
            accountRepo = actRepo;
        }

           
        [AllowAnonymous]                    
        [HttpPost]
        [Route("api/login")]
        public async Task<UserModel> Login([FromBody]  UserModel loginUser)
        {            
            var user = accountRepo.AuthenticateAndLoadUser(loginUser.Username, loginUser.Password);

            if (user == null)
                throw new ApiException("Invalid Login Credentials", 401);


            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
           
            //if (user.Fullname == null)
            //    user.Fullname = string.Empty;
            identity.AddClaim(new Claim("FullName", string.Format("{0} {1}", user.Firstname, user.Lastname)));

            await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            loginUser.Password = string.Empty;
            loginUser.Fullname = string.Format("{0} {1}", user.Firstname, user.Lastname);
            return loginUser;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/logout")]
        public async Task<bool> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);            
            return true;
        }

        [HttpGet]
        [Route("api/isAuthenticated")]
        public bool IsAuthenthenticated()
        {
            return User.Identity.IsAuthenticated;
        }
    }
}
