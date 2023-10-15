using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public class Knowledge
{
    [Key]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    public short LearningTime { get; set; }
    public short RequireTime { get; set; }
    public int CashExpense { get; set; }
    public int Cost { get; set; }
}
