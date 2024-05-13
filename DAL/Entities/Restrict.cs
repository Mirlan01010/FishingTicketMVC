using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Restrict
    {
        public int Id { get; set; }
        public int RestrictedCount { get; set; }
        [ForeignKey("TicketTypeId")]
        public int TicketTypeId { get; set; }
        public virtual TicketType TicketTypeI { get; set; }
        [ForeignKey("WaterBodyId")]
        public int WaterBodyId { get; set; }
        public virtual WaterBody WaterBodyI { get;set; }
    }
}
