using AutoMapper;
using BLL.Models.CitizenShipModels;
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
    public interface ICitizenShipService
    {
        Task<ApiResponse> CreateCitizenShip(CitizenShipRequest model);
        Task<ApiResponse> UpdateCitizenShip(CitizenShipResponse model);

        Task<ApiResponse> DeleteCitizenShip(int? id);
        Task<CitizenShipResponse> GetSingleCitizenShip(int? id);
        Task<List<CitizenShipResponse>> GetAllCitizenShip();
    }
    public class CitizenShipService : ICitizenShipService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CitizenShipService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResponse> CreateCitizenShip(CitizenShipRequest model)
        {
            if (model == null)
            {
                return new ApiResponse() { Message = "No Content", Success = false };
            }
            if (await CheckName(model.Name!))
            {
                return new ApiResponse() { Message = "CitizenShip added allready", Success = false };
            }
            var result = _mapper.Map<CitizenShip>(model);
            _context.CitizenShips.Add(result);
            await _context.SaveChangesAsync();
            return new ApiResponse() { Message = "CitizenShip successfully added :)" };
        }

        public async Task<ApiResponse> DeleteCitizenShip(int? id)
        {
            if (id <= 0) return new ApiResponse() { Message = "No content ID", Success = false };
            var result = await GetSingleCitizenShip(id);
            if (result == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }

            if (await Delete(id)) { return new ApiResponse() { Message = "CitizenShip successfully deleted!" }; }
            return new ApiResponse() { Message = "Error occured....", Success = false };
        }

        public async Task<List<CitizenShipResponse>> GetAllCitizenShip()
        {
            var result = await _context.CitizenShips.ToListAsync();
            return _mapper.Map<List<CitizenShipResponse>>(result);
        }

        public async Task<CitizenShipResponse> GetSingleCitizenShip(int? id)
        {
            var result = await _context.CitizenShips.SingleOrDefaultAsync(u => u.Id == id);
            if (result == null) return null;
            return _mapper.Map<CitizenShipResponse>(result);
        }

        public async Task<ApiResponse> UpdateCitizenShip(CitizenShipResponse model)
        {
            if(model == null) return new ApiResponse() { Message = "No content", Success = false };
            var data = await GetSingleCitizenShip(model.Id);
            if (data == null) { return new ApiResponse() { Message = "Not Found!", Success = false }; }
            if (await Update(model)) { return new ApiResponse() { Message = "CitizenShip updated successfully" }; }
            return new ApiResponse() { Message = "ERROR ACCURED", Success = false };
        }
        //Helper Methods
        private async Task<bool> CheckName(string name)
        {
            var DoesExist = await _context.CitizenShips.Where(h => h.Name!.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
            return DoesExist == null ? false : true;
        }
        private async Task<bool> Delete(int? id)
        {
            var result = _context.CitizenShips.FirstOrDefault(h => h.Id == id);
            _context.CitizenShips.Remove(result!);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> Update(CitizenShipResponse model)
        {
            var result = await _context.CitizenShips.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (result == null) { return false; }
            result.Name = model.Name;
            result.IsActive = model.IsActive;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
