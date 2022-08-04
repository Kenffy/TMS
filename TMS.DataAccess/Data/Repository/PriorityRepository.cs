using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Models;

namespace TMS.DataAccess.Data.Repository
{
    public class PriorityRepository : Repository<CPriority>, IPriorityRepository
    {
        private readonly ApplicationDbContext _db;

        public PriorityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetPriorityListForDropDown()
        {
            return _db.Priorities.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
    }
}
