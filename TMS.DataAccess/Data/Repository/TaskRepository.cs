using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Models;

namespace TMS.DataAccess.Data.Repository
{
    public class TaskRepository : Repository<CTask>, ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetTaskListForDropDown()
        {
            return _db.Tasks.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(CTask task)
        {
            var objFromDb = _db.Tasks.FirstOrDefault(s => s.Id == task.Id);
            objFromDb.Name = task.Name;
            objFromDb.Description = task.Description;
            objFromDb.PriorityId = task.PriorityId;
            objFromDb.UserId = task.UserId;
            if(task.Status != null) { objFromDb.Status = task.Status; }
            objFromDb.TotalOfHours = task.TotalOfHours;
            objFromDb.RequestDate = task.RequestDate;

            _db.SaveChanges();
        }

        public void UpdateTotalOfHours(int taskId)
        {
            var task = _db.Tasks.FirstOrDefault(t => t.Id == taskId);
            var wdays = _db.WDays.Where(w => w.TaskId == task.Id).ToList();
            double hours = 0;
            if(task != null && wdays != null)
            {
                foreach(CDay wday in wdays)
                {
                    hours += wday.Hours;
                }

                task.TotalOfHours = hours;
                _db.SaveChanges();
            }
        }
    }
}
