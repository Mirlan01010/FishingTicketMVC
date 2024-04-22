using AutoMapper;
using BLL.Models.WaterBodyModels;
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
    public interface IWaterBodyService
    {
        Task<ApiResponse> CreateWaterBody(WaterBodyRequest model);
        Task<ApiResponse> UpdateWaterBody(WaterBodyResponse model);

        Task<ApiResponse> DeleteWaterBody(int? id);
        Task<WaterBodyResponse> GetSingleWaterBody(int? id);
        Task<List<WaterBodyResponse>> GetAllWaterBody();
        Task<List<WaterBodyResponse>> GetWaterBodyByRegionId(int id);

    }
    public class WaterBodyService : IWaterBodyService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public WaterBodyService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateWaterBody(WaterBodyRequest model)
        {
            if (model == null)
            {
                return new ApiResponse() { Message = "No Content", Success = false };
            }
            if (await CheckName(model.Name!))
            {
                return new ApiResponse() { Message = "WaterBody added allready", Success = false };
            }
            var result = _mapper.Map<WaterBody>(model);
            _context.WaterBodies.Add(result);
            await _context.SaveChangesAsync();
            return new ApiResponse() { Message = "WaterBody successfully added :)" };
        }

        public async Task<ApiResponse> DeleteWaterBody(int? id)
        {
            if (id <= 0) return new ApiResponse() { Message = "No content ID", Success = false };
            var result = await GetSingleWaterBody(id);
            if (result == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }

            if (await Delete(id)) { return new ApiResponse() { Message = "WaterBody successfully deleted!" }; }
            return new ApiResponse() { Message = "Error occured....", Success = false };
        }

        public async Task<List<WaterBodyResponse>> GetAllWaterBody()
        {
            var result = await _context.WaterBodies.Include(u=>u.Region).ToListAsync();
            return _mapper.Map<List<WaterBodyResponse>>(result);
        }

        public async Task<WaterBodyResponse> GetSingleWaterBody(int? id)
        {
            var result = await _context.WaterBodies.Include(u => u.Region).SingleOrDefaultAsync(u => u.Id == id);
            if (result == null) return null;
            return _mapper.Map<WaterBodyResponse>(result);
        }

        public async Task<List<WaterBodyResponse>> GetWaterBodyByRegionId(int id)
        {
            var result = await _context.WaterBodies.Where(c => c.RegionId == id).ToListAsync();
            return _mapper.Map<List<WaterBodyResponse>>(result);
        }

        public async Task<ApiResponse> UpdateWaterBody(WaterBodyResponse model)
        {
            if (model == null) return new ApiResponse() { Message = "No content", Success = false };
            var data = await GetSingleWaterBody(model.Id);
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
            var result = _context.WaterBodies.FirstOrDefault(h => h.Id == id);
            _context.WaterBodies.Remove(result!);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> Update(WaterBodyResponse model)
        {
            var result = await _context.WaterBodies.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (result == null) { return false; }
            result.Name = model.Name;
            result.IsActive = model.IsActive;
            result.RegionId = model.RegionId;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
