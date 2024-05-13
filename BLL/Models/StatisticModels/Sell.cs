using DAL.Entities.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.StatisticModels
{
    public class Sell
    {
        public ForMonth ForMonth { get; set; }
        public ForDay ForDay { get; set; }
    }
}
