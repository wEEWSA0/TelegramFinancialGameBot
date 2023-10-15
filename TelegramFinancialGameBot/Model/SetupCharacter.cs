using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public class SetupCharacter
{
    [Key]
    public string Id { get; set; } = null!;

    public int Cash { get; set; }
    public int FreeTime { get; set; }

    public List<SetupCharacterCards> Cards { get; set; }
}
