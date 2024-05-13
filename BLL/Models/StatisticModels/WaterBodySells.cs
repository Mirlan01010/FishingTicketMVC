using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.StatisticModels
{
    public class WaterBodySells
    {
        public string Name { get; set; }
        public int TotalCount { get; set; }
        public double TotalPrice { get; set; }
        public List<TicketTypeSells> TicketTypeSellList { get; set; }
    }
}
