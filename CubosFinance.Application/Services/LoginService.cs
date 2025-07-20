using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Login;
using CubosFinance.Application.Exceptions;
using CubosFinance.Application.Settings;
using CubosFinance.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CubosFinance.Application.Services;

public class LoginService : ILoginService
{
    private readonly IPersonRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly JwtSettings _jwt;

    public LoginService(IPersonRepository repository, IConfiguration configuration, IOptions<JwtSettings> jwtOptions)
    {
        _repository = repository;
        _configuration = configuration;
        _jwt = jwtOptions.Value;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var person = await _repository.GetByDocumentAsync(dto.Document);
        if (person == null || !BCrypt.Net.BCrypt.Verify(dto.Password, person.PasswordHash))
            throw new InvalidCredentialsException();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwt.Key);
        var expires = DateTime.UtcNow.AddMinutes(_jwt.ExpiresInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", person.Id.ToString()),
                new Claim(ClaimTypes.Name, person.Name),
            }),
            Expires = expires,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwt.Issuer,
            Audience = _jwt.Audience,
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new LoginResponseDto
        {
            Token = $"Bearer {tokenHandler.WriteToken(token)}"
        };
    }
}
