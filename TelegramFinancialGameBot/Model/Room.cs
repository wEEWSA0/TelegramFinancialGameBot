using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public class Room
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
    public short Step { get; set; }
    public VictoryConditions? VictoryConditions { get; set; }
    public long OwnerChatId { get; set; }
}
