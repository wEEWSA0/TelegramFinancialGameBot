using Microsoft.EntityFrameworkCore;

namespace TelegramFinancialGameBot.Model;

[PrimaryKey(nameof(UserBuisnessId), nameof(ManagerStaffId))]
public class BuisnessManagerStaff
{
    public int UserBuisnessId { get; set; }
    public UserBuisness UserBuisness { get; set; }

    public string ManagerStaffId { get; set; } = null!;
    public ManagerStaff ManagerStaff { get; set; }
}
