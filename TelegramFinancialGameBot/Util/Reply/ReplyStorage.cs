using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot.Util.Reply
{
    internal class ReplyStorage
    {
        public static OutOfRoomStateReplyStorage OutOfRoom = new OutOfRoomStateReplyStorage();
        public static SetupRoomStateReplyStorage SetupRoom = new SetupRoomStateReplyStorage();
        public static InRoomStateReplyStorage InRoom = new InRoomStateReplyStorage();
    }
}
