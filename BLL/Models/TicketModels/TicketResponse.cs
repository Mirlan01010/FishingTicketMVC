using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.TicketModels
{
    public class TicketResponse
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? MiddleName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
        public int RegionId { get; set; }
        public virtual Region? Region { get; set; }
        public int TicketTypeId { get; set; }
        public virtual TicketType? TicketType { get; set; }
        public int WaterBodyId { get; set; }
        public virtual WaterBody? WaterBody { get; set; }
        public int CitizenShipId { get; set; }
        public virtual CitizenShip? CitizenShip { get; set; }
        public string UserId { get; set; }
        public AppUser? appUser { get; set; }
    }
}
