using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TMS.Models
{
    public class CTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public string Customer { get; set; }

        //[DisplayFormat(DataFormatString = "{0:C}")] euro
        [Display(Name = "Total of Hours")]
        public double TotalOfHours { get; set; }

        [Required]
        [Display(Name = "Requested Date")]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ClosedOn { get; set; }

        //[Required]
        [Display(Name = "Priority")]
        public int PriorityId { get; set; }
        [ForeignKey("PriorityId")]
        public virtual CPriority Priority { get; set; }

        //[Required]
        [Display(Name = "Assign To")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
