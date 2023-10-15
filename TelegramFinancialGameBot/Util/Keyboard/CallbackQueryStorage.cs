using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Util.Keyboard;

internal class CallbackQueryStorage
{
    public static readonly OutOfRoomStateCallbackQueryStorage OutOfRoom = new OutOfRoomStateCallbackQueryStorage();
    public static readonly SetupRoomStateCallbackQueryStorage SetupRoom = new SetupRoomStateCallbackQueryStorage();
    public static readonly InRoomStateCallbackQueryStorage InRoom = new InRoomStateCallbackQueryStorage();
}

internal class OutOfRoomStateCallbackQueryStorage
{
    public readonly string YesForInputName = "yes_callback";
    public readonly string NoForInputName = "no_callback";

    public readonly string CreateRoom = "create_room_callback";
    public readonly string JoinInRoom = "join_room_callback";

    public readonly string BackInInputName = "back_name_callback";
}

internal class SetupRoomStateCallbackQueryStorage
{
    public readonly string StartWithoutCharacter = "without_character_callback";

    public readonly string CashIncomeCondition = "cash_icnome_condition_callback";
    public readonly string FreeTimeCondition = "free_time_condition_callback";
    public readonly string BankCondition = "bank_condition_callback";
    public readonly string DreamCondition = "dream_condition_callback";

    public readonly string SetupCurrentConditions = "setup_condition_callback";

    public readonly string Back = "back_callback";
}

internal class InRoomStateCallbackQueryStorage
{
    public readonly string StartWithoutCharacter = "without_character_callback";

    public readonly string Catalog = "catalog_callback";
    public readonly string PlayersStatistic = "players_statistic_callback";
    public readonly string ExitRoom = "exit_room_callback";

    public readonly string Buisness = "buisness_callback";
    public readonly string Property = "property_callback";
    public readonly string Car = "car_callback";
    public readonly string Staff = "staff_callback";
    public readonly string Work = "work_callback";
    public readonly string Dream = "dream_callback";
    public readonly string Accident = "accident_callback";

    public readonly string Yes = "yes_callback";
    public readonly string No = "no_callback";
    public readonly string Back = "back_callback";
    public readonly string BackToMenu = "back_to_menu_callback";

    public readonly PropertyCategoryClass PropertyCategory = new();

    public class PropertyCategoryClass
    {
        public readonly string Sale = "sale_property_callback";
        public readonly string MoveTo = "move_out_property_callback";
        public readonly string MoveIn = "move_in_property_callback";

        public readonly string BuyCard = "buy_property_callback";
        public readonly string BuyAndUseAsHomeCard = "buy_as_home_property_callback";
        public readonly string RentCard = "rent_property_callback";
    }

    public readonly BuisnessCategoryClass BuisnessCategory = new();

    public class BuisnessCategoryClass
    {
        public readonly string Sale = "sale_buisness_callback";
        public readonly string OpenBranch = "open_branch_buisness_callback";
        public readonly string Staff = "staff_buisness_callback";

        public readonly string SaleBuisness = "sale_full_buisness_callback";
        public readonly string SaleOneBranch = "sale_one_branch_buisness_callback";
        public readonly string SaleAnyBranches = "sale_any_branch_buisness_callback";

        public readonly string BuyCard = "buy_buisness_callback";
    }

    public readonly StaffCategoryClass StaffCategory = new();

    public class StaffCategoryClass
    {
        public readonly string Dismiss = "dismiss_staff_callback";

        public readonly string Own = "own_staff_type_callback";
        public readonly string Biz = "biz_staff_type_callback";

        public readonly string StaffType = "staff_type_callback";
        public readonly string ManagerType = "manager_type_callback";
        public readonly string RegionalDirectorType = "regional_director_type_callback";
        public readonly string FinancialDirectorType = "financial_director_type_callback";
        public readonly string GeneralDirecorType = "general_director_type_callback";
        public readonly string FinAndGenDirectorTypes = "financial_director_type_callback";
    }
}
