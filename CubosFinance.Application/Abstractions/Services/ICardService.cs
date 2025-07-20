namespace CubosFinance.Application.Abstractions.Services;

public interface ICardService
{
    Task<CardResponseDto> CreateAsync(Guid accountId, CreateCardDto dto);
    Task<IEnumerable<CardResponseDto>> GetAllByAccountAsync(Guid accountId);
}