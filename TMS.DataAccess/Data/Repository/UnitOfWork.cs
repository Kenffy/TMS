using System;
using System.Collections.Generic;
using System.Text;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Models;

namespace TMS.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Task = new TaskRepository(_db);
            WDay = new WDayRepository(_db);
            Priority = new PriorityRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }

        public ITaskRepository Task { get; private set; }
        public IWDayRepository WDay { get; private set; }
        public IPriorityRepository Priority { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
