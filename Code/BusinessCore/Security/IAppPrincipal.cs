using System;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;

namespace BusinessCore.Security
{
    public interface IAppPrincipal : IPrincipal
    {
        int UserId { get; }
        string Username { get; }
        string Firstname { get; }
        string Surname { get; }
        int CompanyId { get; }

        IPrincipal SetPrincipal(int userId, string username, string firstname, string surname, int companyId);
    }

    public sealed class AppClaims
    {
        private AppClaims() { }
        public const string UserId = "UserId";
        public const string Username  = "Username";
        public const string Firstname = "Firstname";
        public const string Lastname  = "Lastname";
        public const string CompanyId = "CompanyId";
    }
}
