using AutoMapper;
using BLL.Models.TicketTypeModels;
using BLL.Models.Responses;
using DAL.Contexts;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public interface ITicketTypeService
    {
        Task<ApiResponse> CreateTicketType(TicketTypeRequest model);
        Task<ApiResponse> UpdateTicketType(TicketTypeResponse model);

        Task<ApiResponse> DeleteTicketType(int? id);
        Task<TicketTypeResponse> GetSingleTicketType(int? id);
        Task<List<TicketTypeResponse>> GetAllTicketType();
    }
    public class TicketTypeService : ITicketTypeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TicketTypeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateTicketType(TicketTypeRequest model)
        {
            if (model == null)
            {
                return new ApiResponse() { Message = "No Content", Success = false };
            }
            if (await CheckName(model.Name!))
            {
                return new ApiResponse() { Message = "TicketType added allready", Success = false };
            }
            var result = _mapper.Map<TicketType>(model);
            _context.TicketTypes.Add(result);
            await _context.SaveChangesAsync();
            return new ApiResponse() { Message = "TicketType successfully added :)" };
        }

        public async Task<ApiResponse> DeleteTicketType(int? id)
        {
            if (id <= 0) return new ApiResponse() { Message = "No content ID", Success = false };
            var result = await GetSingleTicketType(id);
            if (result == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }

            if (await Delete(id)) { return new ApiResponse() { Message = "TicketType successfully deleted!" }; }
            return new ApiResponse() { Message = "Error occured....", Success = false };
        }

        public async Task<List<TicketTypeResponse>> GetAllTicketType()
        {
            var result = await _context.TicketTypes.ToListAsync();
            return _mapper.Map<List<TicketTypeResponse>>(result);
        }

        public async Task<TicketTypeResponse> GetSingleTicketType(int? id)
        {
            var result = await _context.TicketTypes.SingleOrDefaultAsync(u => u.Id == id);
            if (result == null) return null;
            return _mapper.Map<TicketTypeResponse>(result);
        }

        public async Task<ApiResponse> UpdateTicketType(TicketTypeResponse model)
        {
            if (model == null) return new ApiResponse() { Message = "No content", Success = false };
            var data = await GetSingleTicketType(model.Id);
            if (data == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }
            if (await Update(model)) { return new ApiResponse() { Message = "Updated successfully" }; }
            return new ApiResponse() { Message = "ERROR ACCURED", Success = false };
        }
        //Helper Methods
        private async Task<bool> CheckName(string name)
        {
            var DoesExist = await _context.TicketTypes.Where(h => h.Name!.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
            return DoesExist == null ? false : true;
        }
        private async Task<bool> Delete(int? id)
        {
            var result = _context.TicketTypes.FirstOrDefault(h => h.Id == id);
            _context.TicketTypes.Remove(result!);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> Update(TicketTypeResponse model)
        {
            var result = await _context.TicketTypes.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (result == null) { return false; }
            result.Name = model.Name;
            result.Price = model.Price;
            result.IsActive = model.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
