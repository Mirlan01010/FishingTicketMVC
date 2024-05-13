using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.Statistics
{
    public class ForMonth
    {
        public int Id { get; set; }
        public DateOnly Created { get; set; } = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        public int TotalCount { get; set; }
        public double TotalPrice { get; set; }
    }
}
