using System;
using System.Collections.Generic;
using System.Text;

namespace Booktopia.Domain.Identity
{
    public class AddToRoleModel
    {
        public string Email { get; set; }
        public string selectedRole { get; set; }
        public string selectedUser { get; set; }

        public List<string> roles { get; set; }
        public List<BooktopiaAppUser> users { get; set; }

        public AddToRoleModel()
        {
            roles = new List<string>();
            users = new List<BooktopiaAppUser>();
        }
    }
}
