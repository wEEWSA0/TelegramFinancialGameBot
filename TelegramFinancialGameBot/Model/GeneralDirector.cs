﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public class GeneralDirector
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; } = null!;

    public short CashIncomePercent { get; set; }
    public int CashExpense { get; set; }
    public short TimeIncome { get; set; }
}
