using System;
using System.Collections.Generic;
using System.Text;
using TMS.Models;

namespace TMS.DataAccess.Data.Repository.IRepository
{
    public interface IWDayRepository : IRepository<CDay>
    {
        void Update(CDay wday);
    }
}
