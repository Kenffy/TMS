using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Models;

namespace TMS.DataAccess.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetUserListForDropDown()
        {
            return _db.ApplicationUser.Select(i => new SelectListItem()
            {
                Text = i.FullName,
                Value = i.Id.ToString()
            });
        }

        public void Update(ApplicationUser user)
        {
            var objFromDb = _db.ApplicationUser.FirstOrDefault(s => s.Id == user.Id);
            objFromDb.FirstName = user.FirstName;
            objFromDb.LastName = user.LastName;
            objFromDb.Picture = user.Picture;

            _db.SaveChanges();
        }
    }
}
