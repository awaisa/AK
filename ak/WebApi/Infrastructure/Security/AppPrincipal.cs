using BusinessCore.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace WebApiCore.Infrastructure.Security
{
    public class AppPrincipal : IAppPrincipal
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //public AppPrincipal(IPrincipal principal)
        //{
        //    _principal = principal as ClaimsPrincipal;
        //}
        public AppPrincipal(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            if(_httpContextAccessor.HttpContext!=null)
            _principal = _httpContextAccessor.HttpContext.User;
            else
            {
                _principal = new ClaimsPrincipal
                    (
                        new ClaimsIdentity(new Claim[]
                        {
                            new Claim("UserId", "ClaimsPrincipal_UserId"),
                            new Claim("Username", "ClaimsPrincipal_Username"),
                            new Claim("Firstname", "ClaimsPrincipal_Firstname"),
                            new Claim("Lastname", "ClaimsPrincipal_Lastname"),
                            new Claim("CompanyId", "ClaimsPrincipal_CompanyId")
                        })
                    );
            }
        }
        public int UserId { get
            {
                var val = _principal.FindFirst("UserId")?.Value;
                if (val == null) return 0;
                return Convert.ToInt32(val);
            }
        }

        public string Username
        {
            get
            {
                return _principal.FindFirst("Username")?.Value;
            }
        }
        public string Firstname
        {
            get
            {
                return _principal.FindFirst("Firstname")?.Value;
            }
        }
        public string Surname
        {
            get
            {
                return _principal.FindFirst("Lastname")?.Value;
            }
        }
        public int CompanyId
        {
            get
            {
                var val = _principal.FindFirst("CompanyId")?.Value;
                if (val == null) return 0;
                return Convert.ToInt32(val);
            }
        }
    }
}
