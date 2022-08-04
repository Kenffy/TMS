using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.DataAccess.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        public ITaskRepository Task { get; }
        public IWDayRepository WDay { get; }
        public IPriorityRepository Priority { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        void Save();
    }
}
