using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.RegionModels
{
    public class RegionRequest
    {
        [Required]
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
