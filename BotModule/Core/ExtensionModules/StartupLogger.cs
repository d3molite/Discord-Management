using System.Text;
using Serilog;

namespace BotModule.Core;

public sealed partial class DiscordBot
{
    private readonly StringBuilder _startupLogger = new();

    private void InitializeLogger()
    {
        _startupLogger.AppendLine(new string('#', 50));
    }

    private void LogStartupAction(string input)
    {
        _startupLogger.AppendLine(input);
    }

    private void WriteLog()
    {
        _startupLogger.AppendLine(new string('#', 50));
        Log.Information(_startupLogger.ToString());
        _startupLogger.Clear();
    }
}