using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace BusinessCore.Security
{
    public class TestPrincipal : IAppPrincipal
    {
        private int _userId;
        private string _username;
        private string _firstname;
        private string _surname;
        private int _companyId;

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        public string Surname
        {
            get { return _surname; }
            set { _surname = value; }
        }

        public int CompanyId
        {
            get { return _companyId; }
            set { _companyId = value; }
        }

        public IIdentity Identity => throw new NotImplementedException();

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public IPrincipal SetPrincipal(int userId, string username, string firstname, string surname, int companyId)
        {
            return new TestPrincipal()
            {
                UserId = userId,
                CompanyId = companyId,
                Firstname = firstname,
                Surname = surname,
                Username = username
            };
        }
    }
}
