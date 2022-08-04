using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Models.ViewModels;
using TMS.Utility;

namespace TMS.Pages.Admin.Task
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public TaskViewModel TaskObj { get; set; }
        public void OnGet(int id)
        {
            TaskObj = new TaskViewModel()
            {
                UserList = _unitOfWork.ApplicationUser.GetUserListForDropDown(),
                Priorities = _unitOfWork.Priority.GetPriorityListForDropDown(),
                Task = _unitOfWork.Task.GetFirstOrDefault(includeProperties: "Priority,ApplicationUser", filter: c => c.Id == id)
            };
        }

        public IActionResult OnPostProgress()
        {
            var task = _unitOfWork.Task.GetFirstOrDefault(c => c.Id == TaskObj.Task.Id);
            task.Status = Constant.StatusInProgress;
            _unitOfWork.Save();
            return RedirectToPage("Details", new { id = task.Id});
        }

        public IActionResult OnPostClose()
        {
            var task = _unitOfWork.Task.GetFirstOrDefault(c => c.Id == TaskObj.Task.Id);
            //task.Status = Constant.StatusClosed;
            //_unitOfWork.Save();
            return RedirectToPage("Details", new { id = task.Id });
        }
    }
}