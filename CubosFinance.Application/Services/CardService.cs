using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.Exceptions;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Enums;

namespace CubosFinance.Application.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _repository;
    private readonly IAccountRepository _accountRepository;

    public CardService(ICardRepository repository, IAccountRepository accountRepository)
    {
        _repository = repository;
        _accountRepository = accountRepository;
    }

    public async Task<CardResponseDto> CreateAsync(Guid accountId, CreateCardDto dto)
    {
        if (dto.Cvv.Length != 3 || !dto.Cvv.All(char.IsDigit))
            throw new InvalidCvvException();

        if (dto.Type == CardType.Physical)
        {
            bool physicalExists = await _repository.PhysicalCardExistsAsync(accountId);
            if (physicalExists)
                throw new PhysicalCardAlreadyExistsException();
        }

        var card = new Card
        {
            AccountId = accountId,
            Type = dto.Type,
            Number = dto.Number,
            Cvv = dto.Cvv,
        };

        await _repository.AddAsync(card);

        return new CardResponseDto
        {
            Id = card.Id,
            Type = card.Type,
            Number = card.Number[^4..],
            Cvv = card.Cvv,
            CreatedAt = card.CreatedAt,
            UpdatedAt = card.UpdatedAt
        };
    }
}

