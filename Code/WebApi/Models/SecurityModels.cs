using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore.Models
{
    public class UserIn
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserOut
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string Fullname
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
