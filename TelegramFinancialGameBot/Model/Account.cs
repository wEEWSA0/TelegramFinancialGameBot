using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public class Account
{
    [Key]
    public long ChatId { get; set; }

    public string Name { get; set; }
    public bool isLink { get; set; }
}
