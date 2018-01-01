using System.Collections.Generic;

namespace BusinessCore.Domain.Security
{
    public class User : BaseEntity, ICompanyBaseEntity
    {
        public User()
        {
            Roles = new HashSet<SecurityUserRole>();
        }
        /// <summary>
        /// Username / Email address
        /// </summary>
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }

        public virtual ICollection<SecurityUserRole> Roles { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public bool IsSysAdmin()
        {
            bool isSysAdmin = false;

            if (this.Roles.Count > 0)
            {
                foreach (var role in Roles)
                    if (role.SecurityRole.RoleName == "Administrator")
                    {
                        isSysAdmin = true;
                        break;
                    }
            }

            return isSysAdmin;
        }
    }
}
