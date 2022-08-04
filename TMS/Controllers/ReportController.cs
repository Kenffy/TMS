using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.DataAccess.Data.Repository.IRepository;
using TMS.Utility;

namespace TMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return Json(new { data = _unitOfWork.Task.GetAll(includeProperties: "Priority,ApplicationUser", filter: c => c.UserId == claim.Value && c.Status == Constant.StatusClosed) });
        }

        [HttpPost]
        public IActionResult Get(string date)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            DateTime currdate = Convert.ToDateTime(date);

            var tasks = _unitOfWork.Task.GetAll(includeProperties: "Priority,ApplicationUser", filter: c => c.UserId == claim.Value && c.Status == Constant.StatusClosed && c.ClosedOn.Month == currdate.Month && c.ClosedOn.Year == currdate.Year);

            if(tasks.Count() > 0)
            {
                return Json(new { data = tasks, success = true, message = "Success !" });
            }
            else
            {
                return Json(new { data = tasks, success = false, message = "No task found !" });
            }
        }

    }
}