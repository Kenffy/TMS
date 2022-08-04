using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Utility;
using Constant = TMS.Utility.Constant;

namespace TMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return Json(new { data = _unitOfWork.Task.GetAll(includeProperties: "Priority,ApplicationUser", filter: c => c.UserId == claim.Value && (c.Status == Constant.StatusStarted || c.Status == Constant.StatusInProgress)) });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Task.GetFirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Task.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpPost("{id}")]
        public IActionResult Close(int id)
        {
            var objFromDb = _unitOfWork.Task.GetFirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while closing the task" });
            }
            objFromDb.Status = Constant.StatusClosed;
            objFromDb.ClosedOn = DateTime.Today;
            _unitOfWork.Save();
            return Json(new { success = true, message = "Task closed successful" });
        }
    }
}