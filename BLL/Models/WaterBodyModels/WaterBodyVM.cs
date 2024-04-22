using BLL.Models.RegionModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.WaterBodyModels
{
    public class WaterBodyVM
    {
        public WaterBodyRequest? WaterBodyRequest { get; set; }
        public List<SelectListItem>? regionResponses { get; set; }
    }
}
