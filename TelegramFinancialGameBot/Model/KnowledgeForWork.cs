using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(KnowledgeId), nameof(WorkId))]
public class KnowledgeForWork
{
    public string KnowledgeId { get; set; }
    public Knowledge Knowledge { get; set; }

    public string WorkId { get; set; }
}
