using CubosFinance.Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubosFinance.Application.Abstractions.Services;

public interface IAccountService
{
    Task<AccountResponseDto> CreateAsync(CreateAccountDto dto);
}
