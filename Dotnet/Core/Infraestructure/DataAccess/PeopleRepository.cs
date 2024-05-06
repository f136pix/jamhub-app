using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Infraestructure.DataAccess;

public interface IPeopleRepository
{
    public Task<List<PersonModel>> GetPeopleAsync();
    public Task<PersonModel> InsertPersonAsync(string firstName, string lastName);
    public Task<PersonModel> GetPersonByIdAsync(int id);
}

public class PeopleRepository : IPeopleRepository
{
    private readonly ApplicationDbContext _context;

    public PeopleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PersonModel>> GetPeopleAsync()
    {
        return await _context.People.ToListAsync();
    }

    public async Task<PersonModel> GetPersonByIdAsync(int id)
    {
        return await _context.People.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PersonModel> InsertPersonAsync(string firstName, string lastName)
    {
        var person = new PersonModel { FirstName = firstName, LastName = lastName };
        _context.People.Add(person);
        await _context.SaveChangesAsync();
        return person;
    }
}