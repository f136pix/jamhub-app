using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Infraestructure.DataAccess
{
    public interface IBandRepository
    {
        Task<bool> isBandExistsAsync(string name);
        Task<List<BandModel>> GetBandsAsync();
        Task<BandModel> GetBandByIdAsync(int id);
        Task<BandModel> InsertBandAsync(BandModel bandModel);
    }

    public class BandRepository : IBandRepository
    {
        private readonly ApplicationDbContext _context;

        public BandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> isBandExistsAsync(string name)
        {
            return await _context.Band.AnyAsync(b => b.Name == name);
        }

        public async Task<List<BandModel>> GetBandsAsync()
        {
            return await _context.Band.ToListAsync();
        }

        public async Task<BandModel> GetBandByIdAsync(int id)
        {
            return await _context.Band
                .Include(b => b.Members)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BandModel> InsertBandAsync(BandModel bandModel)
        {
            _context.Band.Add(bandModel);
            await _context.SaveChangesAsync();
            return bandModel;
        }
    }
}