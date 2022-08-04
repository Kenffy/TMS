using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Models.ViewModels;
using TMS.Utility;

namespace TMS.Pages.Admin.Task
{

    [Authorize]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public TaskViewModel TaskObj { get; set; }

        public IActionResult OnGet(int? id)
        {
            TaskObj = new TaskViewModel()
            {
                UserList = _unitOfWork.ApplicationUser.GetUserListForDropDown(),
                Priorities = _unitOfWork.Priority.GetPriorityListForDropDown(),
                Task = new Models.CTask()
            };
            if (id != null)
            {
                TaskObj.Task = _unitOfWork.Task.GetFirstOrDefault(u => u.Id == id);
                if (TaskObj.Task == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (TaskObj.Task.Id == 0)
            {
                TaskObj.Task.CreatedOn = DateTime.Today;
                TaskObj.Task.Status = Constant.StatusStarted;
                _unitOfWork.Task.Add(TaskObj.Task);
            }
            else
            {
                _unitOfWork.Task.Update(TaskObj.Task);
            }
            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }
    }
}