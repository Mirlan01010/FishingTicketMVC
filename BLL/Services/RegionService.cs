using AutoMapper;
using BLL.Models.RegionModels;
using BLL.Models.Responses;
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
    public interface IRegionService {
        Task<ApiResponse> CreateRegion(RegionRequest model);
        Task<ApiResponse> UpdateRegion(RegionResponse model);

        Task<ApiResponse> DeleteRegion(int? id);
        Task<RegionResponse> GetSingleRegion(int? id);
        Task<List<RegionResponse>> GetAllRegion();

    }
    public class RegionService:IRegionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RegionService(AppDbContext context, IMapper mapper)
        {
            _context = context;    
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateRegion(RegionRequest model)
        {
            if(model == null)
            {
                return new ApiResponse() { Message = "No Content", Success = false };
            }
            if (await CheckName(model.Name!))
            {
                return new ApiResponse() { Message = "Region added allready", Success = false };
            }
            var result = _mapper.Map<Region>(model);
            _context.Regions.Add(result);
            await _context.SaveChangesAsync();
            return new ApiResponse() { Message = "Region successfully added :)" };
        }

        public async Task<ApiResponse> DeleteRegion(int? id)
        {
            if (id <= 0) return new ApiResponse() { Message = "No content ID", Success = false };
            var result = await GetSingleRegion(id);
            if (result == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }

            if (await Delete(id)) { return new ApiResponse() { Message = "Region successfully deleted!" }; }
            return new ApiResponse() { Message = "Error occured....", Success = false };
        }

        public async Task<List<RegionResponse>> GetAllRegion()
        {
            var result = await _context.Regions.ToListAsync();
            return _mapper.Map<List<RegionResponse>>(result);
        }
        

        public async Task<RegionResponse> GetSingleRegion(int? id)
        {
            var result = await _context.Regions.SingleOrDefaultAsync(u => u.Id == id);
            if (result == null) return null;
            return _mapper.Map<RegionResponse>(result);
        }

        public async Task<ApiResponse> UpdateRegion(RegionResponse model)
        {
            if (model == null) return new ApiResponse() { Message = "No content", Success = false };
            var data = await GetSingleRegion(model.Id);
            if (data == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }
            if (await Update(model)) { return new ApiResponse() { Message = "Updated successfully"}; }
            return new ApiResponse() { Message = "ERROR ACCURED", Success = false };
        }
        //Helper Methods
        private async Task<bool> CheckName(string name)
        {
            var DoesExist = await _context.Regions.Where(h => h.Name!.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
            return DoesExist == null ? false : true;
        }
        private async Task<bool> Delete(int? id)
        {
            var result = _context.Regions.FirstOrDefault(h => h.Id == id);
            _context.Regions.Remove(result!);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> Update(RegionResponse model)
        {
            var result = await _context.Regions.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (result == null) { return false; }
            result.Name = model.Name;
            result.IsActive = model.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
