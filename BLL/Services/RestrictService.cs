using AutoMapper;
using BLL.Models.Responses;
using BLL.Models.RestrictModels;
using DAL.Contexts;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public interface IRestrictService
    {
        Task<ApiResponse> CreateRestrict(RestrictRequest model);
        Task<ApiResponse> UpdateRestrict(RestrictResponse model);

        Task<ApiResponse> DeleteRestrict(int? id);
        Task<RestrictResponse> GetSingleRestrict(int? id);
        Task<List<RestrictResponse>> GetAllRestrict();
        Task<List<RestrictResponse>> GetRestrictByWaterBodyId(int id);

    }
    public class RestrictService : IRestrictService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RestrictService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateRestrict(RestrictRequest model)
        {
            if (model == null)
            {
                return new ApiResponse() { Message = "No Content", Success = false };
            }
            
            var result = _mapper.Map<Restrict>(model);
            _context.Restricts.Add(result);
            await _context.SaveChangesAsync();
            return new ApiResponse() { Message = "Restrict successfully added :)" };
        }

        public async Task<ApiResponse> DeleteRestrict(int? id)
        {
            if (id <= 0) return new ApiResponse() { Message = "No content ID", Success = false };
            var result = await GetSingleRestrict(id);
            if (result == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }

            if (await Delete(id)) { return new ApiResponse() { Message = "Restrict successfully deleted!" }; }
            return new ApiResponse() { Message = "Error occured....", Success = false };
        }

        public async Task<List<RestrictResponse>> GetAllRestrict()
        {
            var result = await _context.Restricts
                .Include(u => u.WaterBodyI)
                .Include(u => u.TicketTypeI)
                .ToListAsync();
            return _mapper.Map<List<RestrictResponse>>(result);
        }

        public async Task<RestrictResponse> GetSingleRestrict(int? id)
        {
            var result = await _context.Restricts
                .Include(u => u.WaterBodyI)
                .Include(u => u.TicketTypeI)
                .SingleOrDefaultAsync(u => u.Id == id);
            if (result == null) return null;
            return _mapper.Map<RestrictResponse>(result);
        }

        public async Task<List<RestrictResponse>> GetRestrictByWaterBodyId(int id)
        {
            var result = await _context.Restricts
                .Include(u => u.WaterBodyI)
                .Include(u => u.TicketTypeI)
                .Where(c => c.WaterBodyId == id).ToListAsync();
            return _mapper.Map<List<RestrictResponse>>(result);
        }

        public async Task<ApiResponse> UpdateRestrict(RestrictResponse model)
        {
            if (model == null) return new ApiResponse() { Message = "No content", Success = false };
            var data = await GetSingleRestrict(model.Id);
            if (data == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }
            if (await Update(model)) { return new ApiResponse() { Message = "Updated successfully" }; }
            return new ApiResponse() { Message = "ERROR ACCURED", Success = false };
        }
        //Helper Methods
        private async Task<bool> CheckName(string name)
        {
            var DoesExist = await _context.WaterBodies.Where(h => h.Name!.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
            return DoesExist == null ? false : true;
        }
        private async Task<bool> Delete(int? id)
        {
            var result = _context.Restricts.FirstOrDefault(h => h.Id == id);
            _context.Restricts.Remove(result!);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> Update(RestrictResponse model)
        {
            var result = await _context.WaterBodies.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (result == null) { return false; }
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
