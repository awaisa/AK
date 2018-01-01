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
        private ClaimsPrincipal _principal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppPrincipal(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            if (_httpContextAccessor.HttpContext != null)
                _principal = _httpContextAccessor.HttpContext.User;
            else
            {
                SetPrincipal(0, "ClaimsPrincipal_Username", "ClaimsPrincipal_Firstname", "ClaimsPrincipal_Lastname", 0);
            }
        }
        public int UserId
        {
            get
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

        public IIdentity Identity => throw new NotImplementedException();

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IPrincipal SetPrincipal(int userId, string username, string firstname, string surname, int companyId)
        {
            _principal = new ClaimsPrincipal
                    (
                        new ClaimsIdentity(new Claim[]
                        {
                            new Claim(AppClaims.UserId, $"{userId}"),
                            new Claim(AppClaims.Username, $"{username}"),
                            new Claim(AppClaims.Firstname, $"{firstname}"),
                            new Claim(AppClaims.Lastname, $"{surname}"),
                            new Claim(AppClaims.CompanyId, $"{companyId}")
                        })
                    );
            return _principal as IPrincipal;
        }
    }
}
