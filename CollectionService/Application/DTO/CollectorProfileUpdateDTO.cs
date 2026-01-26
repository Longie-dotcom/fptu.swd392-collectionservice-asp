using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class CollectorProfileUpdateDTO
    {
        [Required(ErrorMessage = "Contact info is required")]
        [StringLength(500, ErrorMessage = "Contact info cannot exceed 500 characters")]
        public string ContactInfo { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }
    }
}
