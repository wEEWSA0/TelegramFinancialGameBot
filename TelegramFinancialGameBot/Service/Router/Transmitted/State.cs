namespace TelegramFinancialGameBot.Service.Router.Transmitted;

public sealed class State
{
    private State()
    {

    }

    // limit for public const values per one class
    private const int perLevel = 20;

    private const int OutOfRoomId = 0;
    public static class OutOfRoom
    {
        private const int parent = OutOfRoomId * perLevel;

        public const int CmdStart = parent + 1;

        public const int InputName = parent + 2;
        public const int InputtedNameCorrect = parent + 3;

        public const int CreateOrJoinRoom = parent + 4;
        public const int InputRoomNameForJoinInIt = parent + 5;
        public const int InputRoomNameForCreateIt = parent + 6;
    }

    private const int SetupRoomId = 1;
    public static class SetupRoom
    {
        private const int parent = SetupRoomId * perLevel;

        public const int MainMenu = parent + 1;

        public const int ChangeCashIncome = parent + 2;
        public const int ChangeFreeTime = parent + 3;
    }

    private const int InRoomId = 2;
    public static class InRoom
    {
        private const int parent = InRoomId * perLevel;

        public const int MainMenu = parent + 1;
        public const int OpenCardMenu = parent + 2;
        public const int Profile = parent + 3;
        public const int PlyersInRoomStatistic = parent + 4;

        public const int ChooseCardType = parent + 5;
        public const int InputCardNumber = parent + 6;

        public const int Catalog = parent + 7;

        public static class SetupCharacter
        {
            private const int parent = InRoom.parent * perLevel * 1;

            public const int CreateCharacter = parent + 1;
            //public const int InputDream = parent + 2;
        }

        public static class PropertyCategory
        {
            private const int parent = InRoom.parent * perLevel * 2;

            public const int Info = parent + 1;
            public const int ChooseAction = parent + 2;

            public const int MoveTo = parent + 3;

            public static class Add
            {
                private const int parent = PropertyCategory.parent * perLevel;

                public const int ChooseAction = parent + 1;

                public const int Buy = parent + 2;
                public const int AfterBuy = parent + 3;
                public const int ChooseHowUseProperty = parent + 4;

                public const int Rent = parent + 4;
                public const int AfterRent = parent + 5;
            }
        }

        public static class WorkCategory
        {
            private const int parent = InRoom.parent * perLevel * 3;

            public const int Info = parent + 1;
        }

        public static class BuisnessCategory
        {
            private const int parent = InRoom.parent * perLevel * 4;

            public const int Info = parent + 1;
            public const int ChooseAction = parent + 2;

            public const int Sale = parent + 3;

            public const int SaleOne = parent + 4;
            public const int SaleAny = parent + 5;
            public const int InputCountForSaleAny = parent + 6;
            public const int SaleBuisness = parent + 7;

            public const int Open = parent + 8;
            public const int AfterOpen = parent + 9;

            public static class Add
            {
                private const int parent = BuisnessCategory.parent * perLevel;

                public const int ChooseAction = parent + 1;

                public const int Buy = parent + 2;
                public const int AfterBuy = parent + 3;
                public const int ChooseHowUseProperty = parent + 4;

                public const int Rent = parent + 4;
                public const int AfterRent = parent + 5;
            }
        }

        // хочу знать сколько часов на него потрачу
        public static class StaffCategory
        {
            private const int parent = InRoom.parent * perLevel * 5;

            public const int FullStaffInfo = parent + 1;
            public const int ChooseAction = parent + 2;

            public static class Own
            {
                private const int parent = StaffCategory.parent * perLevel * 1;

                public const int ChooseStaff = parent + 5;
                public const int Info = parent + 1;
                public const int ChooseAction = parent + 2;

                public const int Dismiss = parent + 3;
                public const int AfterDismiss = parent + 4;

                public static class Add
                {
                    private const int parent = Own.parent * perLevel;

                    public const int ChooseAction = parent + 1;

                    public const int Hire = parent + 2;
                }
            }

            public static class Biz
            {
                private const int parent = StaffCategory.parent * perLevel * 2;

                public const int ChooseBuisness = parent + 8;
                public const int ChooseStaffType = parent + 9;
                public const int ChooseStaff = parent + 10;
                public const int Info = parent + 1;
                public const int ChooseAction = parent + 2;

                public const int Dismiss = parent + 3;
                public const int AfterDismiss = parent + 4;

                public const int MoveTo = parent + 5;
                public const int MoveToBuisness = parent + 6;
                public const int AfterMoveTo = parent + 7;

                public static class Add
                {
                    private const int parent = Biz.parent * perLevel;

                    public const int ChooseAction = parent + 1;

                    public const int ChooseBuisness = parent + 2;

                    public const int Hire = parent + 3;
                }
            }
        }
    }
}
