using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Common;
using CubosFinance.Application.DTOs;
using CubosFinance.Application.DTOs.People;
using CubosFinance.Application.Exceptions;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;

namespace CubosFinance.Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IComplianceValidationService _complianceValidationService;

    public PersonService(IPersonRepository personRepository, IComplianceValidationService complianceValidationService)
    {
        _personRepository = personRepository;
        _complianceValidationService = complianceValidationService;
    }

    public async Task<PersonResponseDto> CreateAsync(CreatePersonDto dto)
    {
        if (await _personRepository.ExistsByDocumentAsync(dto.Document))
        {
            throw new DuplicatedDocumentException(dto.Document);
        }

        bool documentoValido = await _complianceValidationService.ValidateAsync(new DocumentValidationRequestDto
        {
            Document = dto.Document
        });

        if (!documentoValido)
            throw new InvalidDocumentException(dto.Document);

        var person = new Person
        {
            Name = dto.Name,
            Document = dto.Document,
            PasswordHash = HashPassword(dto.Password),
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
