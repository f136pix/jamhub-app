using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Infraestructure.DataAccess
{
    public class BandsRepository : ICommonRepository<Band>
    {
        private readonly ApplicationDbContext _context;

        public BandsRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IReadOnlyList<Band>> GetAllAsync()
        {
            return await _context.Bands.ToListAsync();
        }

        public async Task<Band> GetByIdAsync(long id)
        {
            return await _context.Bands
                .Include(b => b.Members)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public Band GetByIdSync(long id)
        {
            return _context.Bands
                .Include(b => b.Members)
                .FirstOrDefault(b => b.Id == id);
        }

        public async Task<Band> AddAsync(Band band)
        {
            _context.Bands.Add(band);
            await _context.SaveChangesAsync();
            return band;
        }

        public Task<Band> UpdateAsync(Band entity)
        {
            throw new NotImplementedException();
        }

        public Task<Band> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<Band> GetByProperty(string propertyName, string value)
        {
            Band? result;

            if (propertyName == "Name")
            {
                result = await _context.Bands
                    .FirstOrDefaultAsync(p => p.Name == value);
            }
            else
            {
                throw new ArgumentException(
                    $"Property '{propertyName}' not found on Bands entity, or not implemented in method.");
            }

            return result;
        }

        public async Task<bool> IsBandExistsAsync(string name)
        {
            return await _context.Bands.AnyAsync(b => b.Name == name);
        }
    }
}