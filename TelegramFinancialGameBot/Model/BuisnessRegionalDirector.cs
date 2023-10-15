using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(UserBuisnessId), nameof(RegionalDirectorId))]
public class BuisnessRegionalDirector
{
    public int UserBuisnessId { get; set; }
    public UserBuisness UserBuisness { get; set; }

    public string RegionalDirectorId { get; set; } = null!;
    public RegionalDirector RegionalDirector { get; set; }
}
