using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.CitizenShipModels
{
    public class CitizenShipResponse
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public bool IsActive { get; set; }

    }
}
