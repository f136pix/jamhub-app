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
        Task<List<Band>> GetBandsAsync();
        Task<Band> GetBandByIdAsync(int id);
        Task<Band> InsertBandAsync(Band band);
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

        public async Task<List<Band>> GetBandsAsync()
        {
            return await _context.Band.ToListAsync();
        }

        public async Task<Band> GetBandByIdAsync(int id)
        {
            return await _context.Band
                .Include(b => b.Members)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Band> InsertBandAsync(Band band)
        {
            _context.Band.Add(band);
            await _context.SaveChangesAsync();
            return band;
        }
    }
}