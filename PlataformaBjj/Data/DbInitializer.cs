using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlataformaBjj.Models;
using PlataformaBjj.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }
            if (_context.Roles.Any(r => r.Name == SD.SuperUser)) return;
            _roleManager.CreateAsync(new IdentityRole(SD.SuperUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.ManagerUser)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.CustomerUser)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                Name = "Javier",
                LastName = "Rodriguez",
                Email = "rodriguezjavie@gmail.com",
                UserName = "rodriguezjavie@gmail.com",
                EmailConfirmed = true
            },"Gds9803274na_").GetAwaiter().GetResult();

            IdentityUser user = await _context.Users.FirstOrDefaultAsync(u => u.Email == "rodriguezjavie@gmail.com");

            await _userManager.AddToRoleAsync(user, SD.SuperUser);
        }
    }
}
