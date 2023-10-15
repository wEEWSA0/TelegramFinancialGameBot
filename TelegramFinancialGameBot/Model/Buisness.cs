using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public class Buisness
{
    [Key]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    public short RequireTime { get; set; }
    public short VariableExpenses { get; set; }
    public int CashIncome { get; set; }
    public int CashExpense { get; set; }
    public int Cost { get; set; }
}
