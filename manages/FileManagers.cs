using System.Collections.Concurrent;

namespace Minty.Log;

public static partial class Logger
{
    private static ConcurrentQueue<string> logs = [];

    private static void addLog(string log)
    {
        logs.Enqueue(log);
    }

    private static void export(string path)
    {
        string fullPath = Path.GetFullPath(path);
        if(!Path.Exists(fullPath)) return;
        string fileString = "";
        while(logs.TryDequeue(out string? log))
        {
            fileString += $"{log}\n";
        }
        File.WriteAllText(path, fileString);
    }

    private static async Task exportAsync(string path)
    {
        string fullPath = Path.GetFullPath(path);
        if(!Path.Exists(fullPath)) return;
        string fileString = "";
        while(logs.TryDequeue(out string? log))
        {
            fileString += $"{log}\n";
        }
        await File.WriteAllTextAsync(path, fileString);
    }

    private static void autoExport(string path)
    {
        string fullPath = Path.GetFullPath(path);
        if(!Path.Exists(fullPath)) return;
        while(true)
        {
            Thread.Sleep(1000);
            string fileString = "";
            while(logs.TryDequeue(out string? log))
            {
                fileString += $"{log}\n";
            }
            File.WriteAllText(fullPath, fileString);
        }
    }

    private static async Task autoExportAsync(string path)
    {
        string fullPath = Path.GetFullPath(path);
        if(!Path.Exists(fullPath)) return;
        while(true)
        {
            await Task.Delay(1000);
            string fileString = "";
            while(logs.TryDequeue(out string? log))
            {
                fileString += $"{log}\n";
            }
            await File.WriteAllTextAsync(fullPath, fileString);
        }
    }
}