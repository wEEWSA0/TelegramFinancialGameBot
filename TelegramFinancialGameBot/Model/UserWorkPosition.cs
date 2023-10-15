using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(UserId), nameof(WorkPositionId))]
public class UserWorkPosition
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int WorkPositionId { get; set; }
    public WorkPosition WorkPosition { get; set; }

    public short Experience { get; set; }
}
