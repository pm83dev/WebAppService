using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public interface IDppMachineRepository
    {
        Task AddAsync(DppMachine dppMachine);
        Task AddRangeAsync(IEnumerable<DppMachine> dppMachines);
        Task<bool> ExistsAsync(string serialNumber);
        Task SaveChangesAsync();
        Task<List<DppMachine>> GetAllMachinesAsync();
        Task DeleteAsync();
        Task<List<string>> GetAllDeviceSerialNumbersAsync(); // Metodo rinominato
    }
    public class DppMachineRepositoryPGSql : IDppMachineRepository
    {
        private readonly AppDbContext _context;

        public DppMachineRepositoryPGSql(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DppMachine dppMachine)
        {
            await _context.DppMachines.AddAsync(dppMachine);
        }

        public async Task AddRangeAsync(IEnumerable<DppMachine> dppMachines)
        {
            await _context.DppMachines.AddRangeAsync(dppMachines);
        }

        public async Task<bool> ExistsAsync(string serialNumber)
        {
            return await _context.DppMachines.AnyAsync(m => m.SerialNumber == serialNumber);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<DppMachine>> GetAllMachinesAsync()
        {
            return await _context.DppMachines.ToListAsync();
        }

        public async Task DeleteAsync()
        {
            await _context.DppMachines.ExecuteDeleteAsync();
        }

        public async Task<List<string>> GetAllDeviceSerialNumbersAsync()
        {
            var deviceIdList = await _context.DppMachines
                .Select(d => d.JobNr)
                .ToListAsync();

            return deviceIdList;
        }
    }
}
