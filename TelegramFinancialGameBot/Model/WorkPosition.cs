using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace TelegramFinancialGameBot.Model;

//[PrimaryKey(nameof(WorkId), nameof(ExpirienceRequire))]
[Index(nameof(WorkId), nameof(ExpirienceRequire), IsUnique = true)]
public class WorkPosition
{
    [Key]
    public int Id { get; set; }

    public string WorkId { get; set; } = null!;
    public Work Work { get; set; } = null!;

    public short ExpirienceRequire { get; set; }
    public int Income { get; set; }
    public short RequireTime { get; set; }
}
