using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public enum AccidentType
{
    Default = 0,
    BuisnessProfitPercent = 1,
    WorkSalary = 2
}
