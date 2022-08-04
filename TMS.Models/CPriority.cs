using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TMS.Models
{
    public class CPriority
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="Priority")]
        public string Name { get; set; }
    }
}
