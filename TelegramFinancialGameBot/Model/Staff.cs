using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public class Staff
{
    [Key]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    public int CashExpense { get; set; }
    public short TimeIncome { get; set; }
}
