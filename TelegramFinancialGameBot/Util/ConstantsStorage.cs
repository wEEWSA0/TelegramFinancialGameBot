namespace TelegramFinancialGameBot.Util;

// Storage for DataStorage in TransmittedData
internal class ConstantsStorage
{
    // CONST
    public static string AccountName =
        "user_name";
    // CONST
    public static string UserId =
        "user_in_room_id";
    // CONST
    public static string RoomName =
        "room_name";
    // CONST
    public static string RoomId =
        "room_id";

    public static string VictoryConditions =
        "victory_conditions";

    public static string Character =
        "setup_character";

    public static string Property =
        "property_user";

    public static string Buisness =
        "buisness_user";

    public static string Staff =
        "buisness_staff";

    public static string Count =
        "count_variable";

    public static string Page =
        "page_number";

    public static string StaffId =
        "staff_id";
}

internal static class CharacterDefaultParamters
{
    public const short DefaultEnergy = 24;
    public const short DefaultFreeTime = 720;

    public const int PenaltyForHomeless = 60000;

    public const int CashForRich = 400000;
    public const int FoodExpenseForRich = 150000;
    public const short FoodExpensePercentForNotRich = 30;
}

internal static class BuisnessConstParamters
{
    public const short EnergyToSale = 15;
    public const short EnergyToOpenBrench = 12;

    public const short StepsToOpen = 3;
}

internal static class StaffConstParamters
{
    public const short EnergyToStaff = 3;
    public const short EnergyToManager = 6;
    public const short EnergyToRegionalDirector = 12;
    public const short EnergyToFinancialDirector = 18;
    public const short EnergyToGeneralDirector = 24;

    public const short StaffsPerPage = 3;

    public const short ManagersPerBranches = 5;
    public const short RegionalDirectorsPerBranches = 30;
}

internal static class SetupCharacterConstants
{
    public const string OwnAndUsesAsHome = "home";
    public const string NotOwnAndRentedHome = "live_rent_home";
    public const string RentOwnHome = "rent_own_home";
}
