using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Infraestructure.DataAccess;

public interface IPeopleRepository
{
    public Task<bool> IsEmailExistsAsync(string email);
    public Task<List<PersonModel>> GetPeopleAsync();
    public Task<PersonModel> InsertPersonAsync(PersonModel personModel);
    public Task<PersonModel> GetPersonByIdAsync(int id);
}

public class PeopleRepository : IPeopleRepository
{
    private readonly ApplicationDbContext _context;

    public PeopleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _context.People.AnyAsync(p => p.Email == email);    }

    public async Task<List<PersonModel>> GetPeopleAsync()
    {
        return await _context.People.ToListAsync();
    }

    public async Task<PersonModel> GetPersonByIdAsync(int id)
    {
        return await _context.People.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PersonModel> InsertPersonAsync(PersonModel personModel)
    {
        _context.People.Add(personModel);
        return personModel;
    }
}