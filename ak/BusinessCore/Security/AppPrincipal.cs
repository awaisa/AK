using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace BusinessCore.Security
{
    public class AppPrincipal
    {
        private readonly ClaimsPrincipal _principal;

        public AppPrincipal(IPrincipal principal)
        {
            _principal = principal as ClaimsPrincipal;
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
