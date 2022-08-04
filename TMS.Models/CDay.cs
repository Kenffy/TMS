using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TMS.Models
{
    public class CDay
    {
        public int Id { get; set; }
        public string Day { get; set; }
        [Required]
        [Display(Name = "Enter a Day")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public DateTime CreatedOn { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Enter Hours")]
        public double Hours { get; set; }
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public virtual CTask Task { get; set; }
    }
}
