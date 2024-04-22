using AutoMapper;
using BLL.Models.CitizenShipModels;
using BLL.Models.RegionModels;
using BLL.Models.TicketModels;
using BLL.Models.TicketTypeModels;
using BLL.Models.WaterBodyModels;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<RegionRequest, Region>();
            CreateMap<Region, RegionResponse>();

            CreateMap<CitizenShipRequest, CitizenShip>();
            CreateMap<CitizenShip, CitizenShipResponse>();

            CreateMap<TicketTypeRequest, TicketType>();
            CreateMap<TicketType, TicketTypeResponse>();

            CreateMap<WaterBodyRequest, WaterBody>();
            CreateMap<WaterBody, WaterBodyResponse>();

            CreateMap<TicketRequest, Ticket>();
            CreateMap<Ticket, TicketResponse>();

        }
    }
}
