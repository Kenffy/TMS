using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Models;

namespace TMS.DataAccess.Data.Repository
{
    public class WDayRepository : Repository<CDay>, IWDayRepository
    {
        private readonly ApplicationDbContext _db;

        public WDayRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CDay wday)
        {
            var objFromDb = _db.WDays.FirstOrDefault(s => s.TaskId == wday.TaskId && (s.Id == wday.Id || s.Date == wday.Date));

            objFromDb.Day = wday.Date.DayOfWeek.ToString();
            objFromDb.Date = wday.Date;
            objFromDb.Hours = wday.Hours;

            _db.SaveChanges();
        }

    }
}
