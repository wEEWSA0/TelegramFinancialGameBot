using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace TelegramFinancialGameBot.Model;

[Index(nameof(UserId), nameof(BuisnessId), IsUnique = true)]
public class UserBuisness
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string BuisnessId { get; set; } = null!;
    public Buisness Buisness { get; set; } = null!;

    public short BranchCount { get; set; }
    public short OpenSteps { get; set; }

    public string? FinancialDirectorId { get; set; }
    public FinancialDirector FinancialDirector { get; set; }

    public string? GeneralDirectorId { get; set; }
    public GeneralDirector GeneralDirector { get; set; }

    //public virtual List<BuisnessManagerStaff> BuisnessManagerStaff { get; set; } = new();
    //public virtual List<BuisnessRegionalDirector> BuisnessRegionalDirectors { get; set; } = new();
}
