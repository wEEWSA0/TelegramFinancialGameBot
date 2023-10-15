using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramFinancialGameBot.Model;

public class Accident
{
    [Key]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int CashExpense { get; set; }
    public short TimeExpense { get; set; }
    public short EnergyCost { get; set; }
    public short StepsDuration { get; set; }
    public int Cost { get; set; }

    [Column("Type")]
    public AccidentType Type { get; set; }
}
