using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

public class Work
{
    [Key]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    public List<KnowledgeForWork> KnowledgeToHire { get; set; }
}
