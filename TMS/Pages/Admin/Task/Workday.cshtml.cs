using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Models;

namespace TMS.Pages.Admin.Task
{
    public class WorkdayModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkdayModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public CDay DayObj { get; set; }

        public IActionResult OnGet(int? taskId, int? id)
        {
            DayObj = new CDay()
            {
                Task = _unitOfWork.Task.GetFirstOrDefault(u => u.Id == taskId),
                TaskId = Convert.ToInt32(taskId)
            };
            if (id != null)
            {
                DayObj = _unitOfWork.WDay.GetFirstOrDefault(includeProperties: "Task", filter: c => c.Id == id);
                if (DayObj == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        //public IActionResult OnGet(int? id)
        //{
        //    DayObj = new CDay()
        //    {
        //        Task = _unitOfWork.Task.GetFirstOrDefault(u => u.Id == id),
        //        TaskId = Convert.ToInt32(id)
        //    };
        //    if (id != null)
        //    {
        //        DayObj = _unitOfWork.WDay.GetFirstOrDefault(u => u.Id == id);
        //        if (DayObj == null)
        //        {
        //            return NotFound();
        //        }
        //    }
        //    return Page();
        //}

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CDay day = _unitOfWork.WDay.GetFirstOrDefault(d => d.Date == DayObj.Date && d.TaskId == DayObj.TaskId);
            
            if (DayObj.Id == 0)
            {
                if (day != null)
                {
                    // TODO Send a warning back if same day added
                    // Currently day will be updated
                    _unitOfWork.WDay.Update(DayObj);
                }
                else
                {
                    DayObj.CreatedOn = DateTime.Today;
                    DayObj.Day = DayObj.Date.DayOfWeek.ToString();
                    _unitOfWork.WDay.Add(DayObj);
                }
                
            }
            else
            {
                _unitOfWork.WDay.Update(DayObj);
            }
            
            _unitOfWork.Save();
            _unitOfWork.Task.UpdateTotalOfHours(DayObj.TaskId);
            return RedirectToPage("./Details", new { id = DayObj.TaskId });
        }
    }
}