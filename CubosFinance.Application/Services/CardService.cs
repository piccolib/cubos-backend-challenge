using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Common;
using CubosFinance.Application.Exceptions;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CubosFinance.Application.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CardService(ICardRepository repository, IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor)
    {
        _cardRepository = repository;
        _accountRepository = accountRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CardResponseDto> CreateAsync(Guid accountId, CreateCardDto dto)
    {
        if (dto.Cvv.Length != 3 || !dto.Cvv.All(char.IsDigit))
            throw new InvalidCvvException();

        if (dto.Type == CardType.Physical)
        {
            bool physicalExists = await _cardRepository.PhysicalCardExistsAsync(accountId);
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

        await _cardRepository.AddAsync(card);

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

    public async Task<IEnumerable<CardResponseDto>> GetAllByAccountAsync(Guid accountId)
    {
        var cards = await _cardRepository.GetAllByAccountIdAsync(accountId);

        return cards.Select(card => new CardResponseDto
        {
            Id = card.Id,
            Type = card.Type,
            Number = card.Number,
            Cvv = card.Cvv,
            CreatedAt = card.CreatedAt,
            UpdatedAt = card.UpdatedAt
        });
    }

    public async Task<PagedResponseDto<CardResponseDto>> GetAllByPersonAsync(Guid personId, int currentPage, int itemsPerPage)
    {
        if (currentPage <= 0) 
            currentPage = 1;

        if (itemsPerPage <= 0) 
            itemsPerPage = 10;

        var cards = await _cardRepository.GetByPersonIdAsync(personId, currentPage, itemsPerPage);

        var cardDtos = cards.Select(c => new CardResponseDto
        {
            Id = c.Id,
            Type = c.Type,
            Number = c.Number[^4..],
            Cvv = c.Cvv,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return new PagedResponseDto<CardResponseDto>
        {
            Data = cardDtos,
            Pagination = new PaginationDto
            {
                ItemsPerPage = itemsPerPage,
                CurrentPage = currentPage,
            }
        };
    }
}

