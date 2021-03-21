using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Concrete;

namespace Blog.MVC.Areas.Admin.Models
{
    public class UserWithRolesViewModal
    {
        public User User{ get; set; }

        public IList<string> Roles { get; set; }
    }
}
