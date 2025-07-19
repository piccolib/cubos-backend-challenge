using CubosFinance.Application.DTOs;
using CubosFinance.Application.DTOs.People;
using CubosFinance.Application.Exceptions;
using CubosFinance.Application.Interfaces;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;

namespace CubosFinance.Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonResponseDto> CreateAsync(CreatePersonDto dto)
    {
        if (await _personRepository.ExistsByDocumentAsync(dto.Document))
        {
            throw new DuplicatedDocumentException(dto.Document);
        }

        var person = new Person
        {
            Name = dto.Name,
            Document = dto.Document,
            PasswordHash = HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _personRepository.AddAsync(person);

        return new PersonResponseDto
        {
            Id = person.Id,
            Name = person.Name,
            Document = person.Document,
            CreatedAt = person.CreatedAt,
            UpdatedAt = person.UpdatedAt
        };
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
