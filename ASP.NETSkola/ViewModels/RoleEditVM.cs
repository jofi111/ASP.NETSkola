using ASP.NETSkola.Models;
using Microsoft.AspNetCore.Identity;

namespace ASP.NETSkola.ViewModels {
    public class RoleEditVM {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }    
    }
}
