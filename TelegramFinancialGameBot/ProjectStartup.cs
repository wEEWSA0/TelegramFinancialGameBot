using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TelegramFinancialGameBot;

public static class ProjectStartup
{
    public async static Task WaitExit(Action<object, EventArgs> processExit)
    {
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(processExit);
        Console.CancelKeyPress += new ConsoleCancelEventHandler(ExitHandler);

        var tcs = new TaskCompletionSource();

        await tcs.Task;
    }

    private static void ExitHandler(object sender, ConsoleCancelEventArgs args)
    {
        args.Cancel = true;

        Environment.Exit(0);
    }
}
