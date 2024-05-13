using AutoMapper;
using BLL.Models.StatisticModels;
using DAL.Contexts;
using DAL.Entities.Statistics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BLL.Services
{
    public interface IStatisticService
    {
        Task CreateForDay(double price);
        Task<ForDay> GetForDay();
        Task<ForMonth> GetForMonth();
        Task<List<TicketTypeSells>> GetTicketTypeSells(DateInterval interval);
        Task<List<WaterBodySells>> GetWaterBodySells(DateInterval interval);

    }
    public class StatisticService:IStatisticService
    {
        private readonly AppDbContext _context;
        public StatisticService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateForDay(double price)
        {
            //var time = DateTime.UtcNow.AddHours(1);
            var time = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);


            if (await _context.ForDays.AnyAsync(u=>u.Created>= time))
            {
                var thisday = await _context.ForDays
                            .Where(u => u.Created >= time)
                            .FirstOrDefaultAsync();
                if (thisday != null)
                {
                    thisday.TotalPrice += price;
                    thisday.TotalCount++;
                    await CreateForMonth(price);
                    await _context.SaveChangesAsync();
                }
                
            }
            else
            {
                await CreateForMonth(price);

                var obj = new ForDay();
                obj.TotalPrice += price;
                obj.TotalCount++;
                _context.ForDays.Add(obj);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ForDay> GetForDay()
        {
            var time = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var result = await _context.ForDays.SingleOrDefaultAsync(u => u.Created >= time);
            return result;
        }

        public async Task<ForMonth> GetForMonth()
        {
            var startOfMonth = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1);
            var result = await _context.ForMonths.SingleOrDefaultAsync(u => u.Created >= startOfMonth);
            return result;
        }

        public async Task<List<TicketTypeSells>> GetTicketTypeSells(DateInterval interval)
        {
            var types = await _context.TicketTypes.ToListAsync();
            var result = new List<TicketTypeSells>();
            if (types != null)
            {
                foreach (var type in types)
                {
                    var temp = new TicketTypeSells { Name = type.Name, TotalCount = await GetCountByTypeId(type.Id,interval) };
                    temp.TotalPrice = temp.TotalCount * type.Price;
                    result.Add(temp);
                }
            }
            return result;
        }

        public async Task<List<WaterBodySells>> GetWaterBodySells(DateInterval interval)
        {
            var waterBodies = await _context.WaterBodies.ToListAsync();
            var result = new List<WaterBodySells>();
            if (waterBodies != null)
            {
                foreach (var item in waterBodies)
                {
                    var temp = new WaterBodySells { Name = item.Name, TotalCount = await GetCountByWaterBodyId(item.Id,interval), TicketTypeSellList = await GetTicketTypeSells(item.Id,interval) };
                    temp.TotalPrice = GetPrice(temp.TicketTypeSellList);
                    result.Add(temp);
                }
            }
            return result;
        }

        private async Task CreateForMonth(double price)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateOnly(today.Year, today.Month, 1);

            if (await _context.ForMonths.AnyAsync(u => u.Created >= startOfMonth))
            {
                
                    var currentMonth = await _context.ForMonths.FirstOrDefaultAsync(u => u.Created >= startOfMonth);
                    if (currentMonth != null)
                    {
                        currentMonth.TotalPrice += price;
                        currentMonth.TotalCount++;
                    }
                
                   
            }
            else
            {
                
                    var month = new ForMonth();
                    month.TotalPrice += price;
                    month.TotalCount ++;
                    _context.ForMonths.Add(month);
                               
            }
        }
        //HElper Methods
        private async Task<int> GetCountByTypeId(int typeId,DateInterval interval)
        {
            return await _context.Tickets
                .Where(u=>u.TicketTypeId == typeId)
                .Where(f=>f.CreatedDate>=interval.Start)
                .Where(f => f.CreatedDate <= interval.End)
                .CountAsync();
        }
        private async Task<int> GetCountByTypeId(int typeId,int WaterbodyId, DateInterval interval)
        {
            return await _context.Tickets
                .Where(f=>f.WaterBodyId == WaterbodyId)
                .Where(u => u.TicketTypeId == typeId)
                .Where(f => f.CreatedDate >= interval.Start)
                .Where(f => f.CreatedDate <= interval.End)
                .CountAsync();
        }
        private async Task<int> GetCountByWaterBodyId(int typeId, DateInterval interval)
        {
            return await _context.Tickets
                .Where(u => u.WaterBodyId == typeId)
                .Where(f=>f.CreatedDate>= interval.Start)
                .Where(f=>f.CreatedDate<= interval.End)
                .CountAsync();
        }
        private async Task<List<TicketTypeSells>> GetTicketTypeSells(int wId, DateInterval interval)
        {
            var types = await _context.TicketTypes.ToListAsync();
            var result = new List<TicketTypeSells>();
            if (types != null)
            {
                foreach (var type in types)
                {
                    var temp = new TicketTypeSells { Name = type.Name, TotalCount = await GetCountByTypeId(type.Id,wId,interval) };
                    temp.TotalPrice = temp.TotalCount * type.Price;
                    result.Add(temp);
                }
            }
            return result;
        }
        private double GetPrice(List<TicketTypeSells> list)
        {
            double result = 0;
            foreach (var ticket in list)
            {
                result += ticket.TotalPrice;
            }
            return result;
        }
    }
}
