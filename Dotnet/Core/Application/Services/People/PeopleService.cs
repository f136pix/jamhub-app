using AutoMapper;
using DemoLibrary.Application.CQRS.Messaging;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Domain.Exceptions;
using DemoLibrary.Domain.Models;
using DemoLibrary.Infraestructure.DataAccess;
using DemoLibrary.Infraestructure.DataAccess.UnitOfWork;
using DemoLibrary.Models;
using Microsoft.VisualBasic.CompilerServices;

namespace DemoLibrary.Application.Services.People;

public interface IPeopleService
{
    Task<Person> RegisterUserAsync(CreatePersonDto dto);
    Task<bool> ConfirmUserAsync(ConfirmEmailCommand dto);
}

public class PeopleService : IPeopleService
{
    private readonly IPeopleRepository _peopleRepository;
    private readonly IConfirmationTokenRepository _confirmationTokenRepository;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PeopleService(
        IPeopleRepository peopleRepository,
        IConfirmationTokenRepository confirmationTokenRepository,
        IUnitOfWork uow,
        IMapper mapper)
    {
        _peopleRepository = peopleRepository;
        _confirmationTokenRepository = confirmationTokenRepository;
        _uow = uow;
        _mapper = mapper;
    }

    // adds user and confirmation token transactionally
    public async Task<Person> RegisterUserAsync(CreatePersonDto dto)
    {
        var person = _mapper.Map<Person>(dto);
        await _peopleRepository.InsertPersonAsync(person);

        if (dto.ConfirmationToken != null)
        {
            var confirmationToken = new ConfirmationToken
            {
                Token = dto.ConfirmationToken,
                UserId = person.Id
            };

            await _confirmationTokenRepository.InsertConfirmationTokenAsync(confirmationToken);
        }

        await _uow.CommitAsync();

        return person;
    }

    public async Task<bool> ConfirmUserAsync(ConfirmEmailCommand dto)
    {
        var wasValidated = await _confirmationTokenRepository.ConfirmUserByTokenAsync(dto.ConfirmationToken);

        try
        {
            await _uow.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new CouldNotValidateUser("Could not validate user");
        }

        return true;
    }
}