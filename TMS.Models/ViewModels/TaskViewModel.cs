using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Models.ViewModels
{
    public class TaskViewModel
    {
        public CTask Task { get; set; }
        public IEnumerable<SelectListItem> Priorities { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }
    }
}
