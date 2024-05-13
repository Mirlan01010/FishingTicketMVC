using AutoMapper;
using BLL.Models.TicketModels;
using BLL.Models.Responses;
using DAL.Contexts;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public interface ITicketService
    {
        Task<ApiResponse> CreateTicket(TicketRequest model, string userId);
        Task<ApiResponse> UpdateTicket(TicketResponse model);

        Task<ApiResponse> DeleteTicket(int id);
        Task<TicketResponse> GetSingleTicket(int id);
        Task<List<TicketResponse>> GetAllTicketByWaterBodyId(int id);

        Task<List<TicketResponse>> GetAllTicket();
    }
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly IStatisticService _statisticService;

        public TicketService(AppDbContext context, IMapper mapper, UserManager<ExtendedIdentityUser> userManager, IStatisticService statisticService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _statisticService = statisticService;
        }

        public async Task<ApiResponse> CreateTicket(TicketRequest model,string userId)
        {
            if (model == null)
            {
                return new ApiResponse() { Message = "No Content", Success = false };
            }
            //if (await CheckName(model.Name!))
            //{
            //    return new ApiResponse() { Message = "Ticket added allready", Success = false };
            //}
            var result = _mapper.Map<Ticket>(model);

            var price = await _context.TicketTypes.FirstOrDefaultAsync(t => t.Id==result.TicketTypeId);
            if (price != null)
            {
                await _statisticService.CreateForDay(price.Price);
            }
            result.StartTime=result.StartTime.ToUniversalTime();
            result.EndTime=result.EndTime.ToUniversalTime();
            result.UserId=userId;
            _context.Tickets.Add(result);
            await _context.SaveChangesAsync();

            return new ApiResponse() { Message = "Ticket successfully added :)" };
        }

        public async Task<ApiResponse> DeleteTicket(int id)
        {
            if (id <= 0) return new ApiResponse() { Message = "No content ID", Success = false };
            var result = await GetSingleTicket(id);
            if (result == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }

            if (await Delete(id)) { return new ApiResponse() { Message = "Ticket successfully deleted!" }; }
            return new ApiResponse() { Message = "Error occured....", Success = false };
        }

        public async Task<List<TicketResponse>> GetAllTicket()
        {
            var result = await _context.Tickets
                .Include(u=>u.TicketType)
                .Include(u => u.Region)
                .Include(u => u.WaterBody)
                .Include(u => u.CitizenShip)
                .Include(u => u.TicketType).ToListAsync();

            var tickets =  _mapper.Map<List<TicketResponse>>(result);
            

            foreach (var ticket in tickets)
            {
                var user = await _userManager.FindByIdAsync(ticket.UserId);
                if (user == null) return null;
                var userInfo = new AppUser { Name = user.UserName, Email = user.Email };
                ticket.appUser = userInfo;
                return tickets;
            }
            return tickets;
        }

        public async Task<List<TicketResponse>> GetAllTicketByWaterBodyId(int id)
        {
            var result = await _context.Tickets.Where(u=>u.WaterBodyId==id).ToListAsync();
            return _mapper.Map<List<TicketResponse>>(result);
        }

        public async Task<TicketResponse> GetSingleTicket(int id)
        {
            var result = await _context.Tickets.SingleOrDefaultAsync(u => u.Id == id);
            if (result == null) return null;
            return _mapper.Map<TicketResponse>(result);
        }

        public async Task<ApiResponse> UpdateTicket(TicketResponse model)
        {
            if (model == null) return new ApiResponse() { Message = "No content", Success = false };
            var data = await GetSingleTicket(model.Id);
            if (data == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }
            if (await Update(model)) { return new ApiResponse() { Message = "Updated successfully" }; }
            return new ApiResponse() { Message = "ERROR ACCURED", Success = false };
        }
        //Helper Methods
        //private async Task<bool> CheckName(string name)
        //{
        //    var DoesExist = await _context.Tickets.Where(h => h.Name!.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
        //    return DoesExist == null ? false : true;
        //}
        private async Task<bool> Delete(int id)
        {
            var result = _context.Tickets.FirstOrDefault(h => h.Id == id);
            _context.Tickets.Remove(result!);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> Update(TicketResponse model)
        {
            var result = await _context.Tickets.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (result == null) { return false; }
            result.FirstName = model.FirstName;
            result.LastName = model.LastName;
            result.MiddleName = model.MiddleName;
            result.RegionId = model.RegionId;
            result.TicketTypeId = model.TicketTypeId;
            result.CitizenShipId = model.CitizenShipId;
            result.WaterBodyId = model.WaterBodyId;
            result.IsActive = model.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
