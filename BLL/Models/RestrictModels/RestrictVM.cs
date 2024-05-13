using BLL.Models.WaterBodyModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.RestrictModels
{
    public class RestrictVM
    {
        public RestrictRequest? restrictRequest { get; set; }
        public List<SelectListItem>? TicketTypeResponse { get; set; }

        public List<SelectListItem>? WaterbodyResponse { get; set; }
    }
}
