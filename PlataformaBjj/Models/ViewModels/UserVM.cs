using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models.ViewModels
{
    public class UserVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<UserType> UserTypesList { get; set; }
    }
}
