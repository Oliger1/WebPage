using System;
using Serilog;

public class LogService
{
    private readonly string _logFilePath;

    public LogService(string logFilePath)
    {
        _logFilePath = logFilePath;
        InitializeLogger();
    }

    private void InitializeLogger()
    {
        // Kontrollo nëse folderi ekziston
        if (!System.IO.Directory.Exists(_logFilePath))
        {
            // Krijoni folderin nëse nuk ekziston
            System.IO.Directory.CreateDirectory(_logFilePath);
        }
    }

    public void LogInformation(string message)
    {
        Log.Information(message);
    }

    public void LogError(string message)
    {
        Log.Error(message);
    }

    public void LogWarning(string message)
    {
        Log.Warning(message);
    }

    public void LogDebug(string message)
    {
        Log.Debug(message);
    }

    public void LogFatal(string message)
    {
        Log.Fatal(message);
    }
}
