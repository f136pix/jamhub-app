using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using DemoLibrary.Application.DataAccess;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Infraestructure.DataAccess;

public class PeopleRepository : ICommonRepository<Person>
{
    private readonly ApplicationDbContext _context;

    public PeopleRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<IReadOnlyList<Person>> GetAllAsync()
    {
        return await _context.People.ToListAsync();
    }

    public async Task<Person> AddAsync(Person entity)
    {
        await _context.People.AddAsync(entity);
        return entity;
    }

    public async Task<Person> UpdateAsync(Person entity)
    {
        var person = await _context.People.FindAsync(entity.Id);
        person = entity;

        return person;
    }

    public async Task<Person> GetByIdAsync(long id)
    {
        return await _context.People
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Person GetByIdSync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<Person> DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Person> GetByProperty(string propertyName, string value)
    {
        var propertyInfo = typeof(Person).GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property '{propertyName}' not found on Person entity.");
        }
        
        return await _context.People
            .FirstOrDefaultAsync(p => p.GetType().GetProperty(propertyName).GetValue(p).ToString() == value);
    }
}