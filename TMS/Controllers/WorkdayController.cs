using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.DataAccess.Data.Repository.IRepository;


namespace TMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkdayController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkdayController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get(int? id)
        {
            return Json(new { data = _unitOfWork.WDay.GetAll(includeProperties: "Task", filter: c => c.TaskId == id) });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.WDay.GetFirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.WDay.Remove(objFromDb);
            _unitOfWork.Save();
            _unitOfWork.Task.UpdateTotalOfHours(objFromDb.TaskId);
            return Json(new { success = true, message = "Delete successful" });
        }
    }
}
