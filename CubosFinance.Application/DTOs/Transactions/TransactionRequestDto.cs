using CubosFinance.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubosFinance.Application.DTOs.Transactions;

public class TransactionResquestDto
{
    public int ItemsPerPage { get; set; } = 10;
    public int CurrentPage { get; set; } = 1;
    public TransactionType? Type { get; set; }
}

