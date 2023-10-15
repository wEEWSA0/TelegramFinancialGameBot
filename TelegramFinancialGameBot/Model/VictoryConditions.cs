using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public class VictoryConditions
{
    [Key]
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public int CashIncome { get; set; }
    public short RequireTime { get; set; }
    public short TimeForPaymentsToBank { get; set; }
    public bool ShouldDreamBeCompleted { get; set; }

    public VictoryConditions()
    {
        CashIncome = 350000;
        RequireTime = 250;
        TimeForPaymentsToBank = 6;
        ShouldDreamBeCompleted = true;
    }
}
