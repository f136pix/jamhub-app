using AutoMapper;
using DemoLibrary.Application.CQRS.Messaging;
using DemoLibrary.Application.Dtos.People;
using DemoLibrary.Business.Exceptions;
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
        if (await _peopleRepository.IsEmailExistsAsync(dto.Email))
            throw new AlreadyExistsException("Email");

        if (dto.Id.HasValue && await _peopleRepository.GetPersonByIdAsync(dto.Id.Value) != null)
        {
            throw new AlreadyExistsException("Id");
        }

        var person = _mapper.Map<Person>(dto);
        await _peopleRepository.InsertPersonAsync(person);

        if (dto.ConfirmationToken == null)
        {
            throw new Exception("Cant create user, no confirmation token provided");
        }

        var confirmationToken = new ConfirmationToken
        {
            Token = dto.ConfirmationToken,
            UserId = person.Id
        };

        await _confirmationTokenRepository.InsertConfirmationTokenAsync(confirmationToken);

        await _uow.CommitAsync();

        return person;
    }

    public async Task<bool> ConfirmUserAsync(ConfirmEmailCommand dto)
    {
        //  too much business logic delegated to repository
        //     var wasValidated = await _confirmationTokenRepository.ConfirmUserByTokenAsync(dto.ConfirmationToken);
        //
        //     await _uow.CommitAsync();
        //
        //     return true;


        // businsess login mostly in service
        var user = await _confirmationTokenRepository.GetConfirmationTokenByToken(dto.ConfirmationToken)
            .ContinueWith(token => token.Result.User);

        if (user == null)
        {
            throw new CouldNotValidateUser("Invalid token");
        }

        if (user.Verified)
        {
            throw new CouldNotValidateUser("User already verified");
        }

        user.Verified = true;

        await _uow.CommitAsync();

        return true;
    }
}