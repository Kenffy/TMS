using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using TMS.Models;

namespace TMS.DataAccess.Data.Repository.IRepository
{
    public interface IPriorityRepository : IRepository<CPriority>
    {
        IEnumerable<SelectListItem> GetPriorityListForDropDown();
    }
}
