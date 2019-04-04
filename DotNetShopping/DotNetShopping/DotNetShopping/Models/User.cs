using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetShopping.Models
{
    public class UserListModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
    public class UserRolesModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}