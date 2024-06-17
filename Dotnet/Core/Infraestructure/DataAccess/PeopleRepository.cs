using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Infraestructure.DataAccess;

public interface IPeopleRepository
{
    public Task<bool> IsEmailExistsAsync(string email);
    public Task<List<Person>> GetPeopleAsync();
    public Task<Person> InsertPersonAsync(Person person);
    public Task<Person> GetPersonByIdAsync(int id);
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
        return await _context.People.AnyAsync(p => p.Email == email);
    }

    public async Task<List<Person>> GetPeopleAsync()
    {
        return await _context.People.ToListAsync();
    }

    public async Task<Person> GetPersonByIdAsync(int id)
    {
        return await _context.People.Include(p => p.Bands)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Person> InsertPersonAsync(Person person)
    {
        if (await IsEmailExistsAsync(person.Email))
        {
            throw new AlreadyExistsException("Email");
        }

        _context.People.Add(person);
        return person;
    }
}