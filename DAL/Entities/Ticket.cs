using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? MiddleName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateOnly CreatedDate { get; set; }= new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

        public bool IsActive { get; set; } = true;
        [ForeignKey("RegionId")]
        public int RegionId { get; set; }
        public virtual Region? Region { get; set; }
        [ForeignKey("TicketTypeId")]
        public int TicketTypeId { get; set; }
        public virtual TicketType? TicketType { get; set; }
        [ForeignKey("WaterBodyId")]
        public int WaterBodyId { get; set; }
        public virtual WaterBody? WaterBody { get; set; }
        [ForeignKey("CitizenShipId")]
        public int CitizenShipId { get; set; }
        public virtual CitizenShip? CitizenShip { get; set; }
        public string UserId { get; set; }
    }
}
