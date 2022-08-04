using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using TMS.Models;

namespace TMS.DataAccess.Data.Repository.IRepository
{
    public interface ITaskRepository : IRepository<CTask>
    {
        IEnumerable<SelectListItem> GetTaskListForDropDown();

        void UpdateTotalOfHours(int taskId);
        void Update(CTask task);
    }
}
