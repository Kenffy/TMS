using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Models.ViewModels
{
    public class ReportViewModel
    {
        public IEnumerable<CTask> Tasks { get; set; }
        [Display(Name ="Current Month")]
        [DataType(DataType.Date)]
        public DateTime CurrentDate { get; set; }
    }
}
