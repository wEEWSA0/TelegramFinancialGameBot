using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(UserId), nameof(StaffId))]
public class UserStaff
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string StaffId { get; set; } = null!;
    public Staff Staff { get; set; } = null!;
}
