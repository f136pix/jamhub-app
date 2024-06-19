using DemoLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Business.Exceptions;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Infraestructure.DataAccess;

public interface IPeopleRepository
{
    public Task<bool> IsEmailExistsAsync(string email);
    public Task<List<Person>> GetPeopleAsync();
    public Task<Person> InsertPersonAsync(Person person);
    public Task<Person> GetPersonByIdAsync(int id);
    public Task<Person> UpdatePersonAsync(UpdatePersonDto dto);
}

public class PeopleRepository : IPeopleRepository
{
    private readonly IBandRepository _bandRepository;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PeopleRepository(IBandRepository bandRepository, ApplicationDbContext context, IMapper mapper)
    {
        _bandRepository = bandRepository;
        _context = context;
        _mapper = mapper;
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

    public async Task<Person> UpdatePersonAsync(UpdatePersonDto dto)
    {
        var person = await GetPersonByIdAsync(dto.Id);

        if (person == null)
            throw new PersonNotFoundException(dto.Id);

        // person.FirstName = dto.FirstName;
        // person.LastName = dto.LastName;
        // person.Instrument = dto.Instrument;
        // person.CellphoneNumber = dto.CellphoneNumber;
        // person.CityName = dto.CityName;

        // updates our person
        _mapper.Map(dto, person);

        // ads the person to the recieved bands
        if (dto.BandIds != null)
        {
            foreach (var bandId in dto.BandIds)
            {
                await _bandRepository.GetBandByIdAsync(bandId).ContinueWith(task =>
                {
                    var band = task.Result;

                    if (band == null)
                    {
                        throw new BandNotFoundException(bandId);
                    }

                    person.Bands.Add(band);
                });
            }
        }

        return person;
    }
}