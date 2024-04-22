using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.WaterBodyModels
{
    public class WaterBodyUpdateVM
    {
        public WaterBodyResponse? WaterBodyResponse { get; set; }
        public List<SelectListItem>? regionResponses { get; set; }
    }
}
