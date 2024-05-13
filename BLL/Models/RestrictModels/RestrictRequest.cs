using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.RestrictModels
{
    public class RestrictRequest
    {
        public int RestrictedCount { get; set; }
        public int TicketTypeId { get; set; }
        public int WaterBodyId { get; set; }
    }
}
