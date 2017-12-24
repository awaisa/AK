using System;
using System.Net.Http;
using System.Security.Claims;

namespace BusinessCore.Security
{
    public interface IAppPrincipal
    {
        int UserId { get; }
        string Username { get; }
        string Firstname { get; }
        string Surname { get; }
        int CompanyId { get; }
    }
}
