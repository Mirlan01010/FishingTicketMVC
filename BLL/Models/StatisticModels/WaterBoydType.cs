using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.StatisticModels
{
    public class WaterBoydType
    {
        public List<TicketTypeSells> TypeSellsList {  get; set; }
        public List<WaterBodySells> WaterBodySellsList { get; set; }
    }
}
