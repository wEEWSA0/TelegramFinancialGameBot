using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(UserId), nameof(PropertyId))]
public class UserProperty
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string PropertyId { get; set; } = null!;
    public Property Property { get; set; } = null!;

    public bool UsesAsHome { get; set; }
    public bool IsOwner { get; set; }
}
