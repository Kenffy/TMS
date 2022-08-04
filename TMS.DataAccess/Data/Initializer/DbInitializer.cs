using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMS.Models;
using TMS.Utility;

namespace TMS.DataAccess.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_db.Roles.Any(r => r.Name == Constant.ManagerRole)) return;

            _roleManager.CreateAsync(new IdentityRole(Constant.ManagerRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constant.ConsultantRole)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                FirstName = "Soft",
                LastName = "Kenffy"
            }, "Admin420*").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUser.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();

            _userManager.AddToRoleAsync(user, Constant.ManagerRole).GetAwaiter().GetResult();

            // Add Priorities
            List<CPriority> priorities = new List<CPriority>();
            priorities.Add(new CPriority() { Name = Constant.HighPriority });
            priorities.Add(new CPriority() { Name = Constant.MediumPriority });
            priorities.Add(new CPriority() { Name = Constant.LowPriotiy });

            _db.Priorities.AddRange(priorities);
            _db.SaveChanges();
        }
    }
}
