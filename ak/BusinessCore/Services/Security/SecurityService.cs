using BusinessCore.Data;
using BusinessCore.Domain.Security;
using System.Collections.Generic;
using System.Linq;
using BusinessCore.Domain;
using System.Security.Principal;

namespace BusinessCore.Services.Security
{
    public class SecurityService : ISecurityService
    {
        ApplicationContext _dbcontext;
        IRepository<User> _userRepo;
        IRepository<SecurityRole> _securityRoleRepo;
        IRepository<SecurityPermission> _securityPermissionRepo;
        IRepository<SecurityGroup> _securityGroupRepo;
        IRepository<SecurityUserRole> _securityUserRoleRepo;
        IRepository<SecurityRolePermission> _securityRolePermissionRepo;
        IRepository<Company> _companyRepo;

        public SecurityService(ApplicationContext dbcontext, IRepository<User> userRepo = null,
            IRepository<SecurityRole> securityRoleRepo = null,
            IRepository<SecurityPermission> securityPermissionRepo = null,
            IRepository<SecurityGroup> securityGroupRepo = null,
            IRepository<SecurityUserRole> securityUserRoleRepo = null,
            IRepository<SecurityRolePermission> securityRolePermissionRepo = null,
            IRepository<Company> companyRepo = null)
        {
            _dbcontext = dbcontext;
            _userRepo = userRepo;
            _securityRoleRepo = securityRoleRepo;
            _securityPermissionRepo = securityPermissionRepo;
            _securityGroupRepo = securityGroupRepo;
            _securityUserRoleRepo = securityUserRoleRepo;
            _securityRolePermissionRepo = securityRolePermissionRepo;
            _companyRepo = companyRepo;
        }

        public IEnumerable<SecurityRolePermission> GetPermissionsForRole(string rolename)
        {
            var role = GetRole(rolename);

            if (role.SysAdmin)
                return _securityRolePermissionRepo.Table.AsEnumerable();

            return role.Permissions.AsEnumerable();
        }

        public IEnumerable<SecurityUserRole> GetRolesForUser(string username)
        {
            var user = GetUser(username);
            return user.Roles;
        }

        public SecurityPermission GetSecurityPermission(int securityPermissionId)
        {
            return _securityPermissionRepo.Table.Where(p => p.Id == securityPermissionId).FirstOrDefault();
        }

        public SecurityRole GetSecurityRole(int securityRoleId)
        {
            return _securityRoleRepo.Table.Where(r => r.Id == securityRoleId).FirstOrDefault();
        }

        public IEnumerable<SecurityRole> GetAllSecurityRole()
        {
            return _securityRoleRepo.Table.AsEnumerable();
        }

        public User GetUser(string username)
        {
            return _userRepo.Table.Where(u => u.Username == username).FirstOrDefault();
        }

        public SecurityRole GetRole(string rolename)
        {
            return _securityRoleRepo.Table.Where(r => r.RoleName == rolename).FirstOrDefault();
        }

        public bool CheckPermission(string permissionName, string username)
        {
            bool hasPermission = false;

            var user = GetUser(username);

            foreach (var role in user.Roles)
            {
                if (role.SecurityRole.Permissions.Where(p => p.SecurityPermission != null && p.SecurityPermission.PermissionName == permissionName).FirstOrDefault() != null)
                {
                    hasPermission = true;
                    break;
                }
            }

            return hasPermission;
        }

        public IEnumerable<SecurityUserRole> GetUsersInSecurityRole(string securityRoleName)
        {
            var role = GetRole(securityRoleName);

            return role != null ? role.Users.AsEnumerable() : null;
        }

        public void AddUserInRole(int userId, int securityRoleId)
        {
            var item = new SecurityUserRole()
            {
                UserId = userId,
                SecurityRoleId = securityRoleId
            };

            _securityUserRoleRepo.Insert(item);
        }

        public void RemoveUserInRole(int userId, int roleId)
        {
            var itemToDelete = _securityUserRoleRepo.Table.ToList().Where(i => i.SecurityRoleId == roleId && i.UserId == userId).FirstOrDefault();

            _securityUserRoleRepo.Delete(itemToDelete);
        }

        public void AddPermissionToRole(int roleId, int permissionId)
        {
            var securityRolePermission = new SecurityRolePermission()
            {
                SecurityPermissionId = permissionId,
                SecurityRoleId = roleId
            };

            _securityRolePermissionRepo.Insert(securityRolePermission);
        }

        public IEnumerable<string> GetPermissionsForUser(string username)
        {
            var user = GetUser(username);

            if (user == null)
                return null;

            if (user.IsSysAdmin())
            {
                var permissionsSysAdmin = from p in _securityPermissionRepo.Table
                                          select p.PermissionName;

                return permissionsSysAdmin;
            }

            var roles = GetRolesForUser(username);

            var permissions = roles.SelectMany(r => r.SecurityRole.Permissions).ToList().AsQueryable();

            var all = from p in permissions
                      select p.SecurityPermission.PermissionName;

            return all;
        }

        public IEnumerable<SecurityGroup> GetAllSecurityGroup()
        {
            return _securityGroupRepo.Table.AsEnumerable();
        }

        public List<int> GetPermissionByRoleId(int securityRoleId)
        {
            return _securityRolePermissionRepo.Table.Where(x => x.SecurityRoleId == securityRoleId).Select(x => x.SecurityPermissionId).ToList();
        }

        //public void RemoveRolePermission(int roleId)
        //{
        //    var rolePermission = _securityRolePermissionRepo.Table.Where(x => x.SecurityRoleId == roleId).AsEnumerable();

        //    _securityRolePermissionRepo.Delete(rolePermission);
        //}

        public IEnumerable<User> GetAllUser()
        {
            return _userRepo.Table.AsEnumerable();
        }

        public void AddRole(string roleName, int roleId)
        {
            if (roleId > 0)
            {
                var entity = _securityRoleRepo.GetById(roleId);
                entity.RoleName = roleName;

                _securityRoleRepo.Update(entity);
            }
            else
            {
                var role = new SecurityRole()
                {
                    RoleName = roleName,
                    SysAdmin = false
                };

                _securityRoleRepo.Insert(role);
            }
        }

        public void DeleteRole(int roleId)
        {
            //RemoveRolePermission(roleId);
            var entity = _securityRoleRepo.GetById(roleId);

            _securityRoleRepo.Delete(entity);
        }

        public void AddUser(string username /*, string email**/, string firstname, string lastname)
        {

            var company = new Company()
            {
                Name = "Financial Solutions Inc.",
                CompanyCode = "100",
                ShortName = "FSI",
            };

            _companyRepo.Insert(company);

            var user = new User()
            {
                Username = username,
                //Username = email,
                Firstname = firstname,
                Lastname = lastname,
                CompanyId = company.Id
            };
            _userRepo.Insert(user);
        }

        public User CurrentUser
        {
            get { return _userRepo.Table.Where(u => u.Username == WindowsIdentity.GetCurrent().Name).FirstOrDefault(); }
        }

        public int UserCompanyId
        {
            get
            {
                int companyId = 0;
                var user = CurrentUser;
                if (user != null)
                    companyId = user.CompanyId;
                return companyId;
            }
        }

        public bool Authenticate(string username, string password)
        {
            var user = _userRepo.Table.FirstOrDefault(u =>
            u.Username == username &&
            u.Password == password);
            if (user != null)
                return true;
            return false;
        }
        public User AuthenticateAndLoadUser(string username, string password)
        {
            var user = _dbcontext.Users.FirstOrDefault(u =>
            u.Username == username &&
            u.Password == password);
            return user;
        }
    }
}
