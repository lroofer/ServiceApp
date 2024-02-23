using System.Diagnostics;
using System.Text.Json;
using DetailsManager;
using static DetailsManager.Markup;

namespace ServiceApp;

internal static class Program
{
    static int SelectMethodMenu()
    {
        var menu = new Menu(new []
        {
            "Enter an absolute path to the file [recommended]",
            "Use FileManager to select a file"
        }, "[DetailsManager]");
        return menu.Run();
    }

    static async Task<List<Widget>?> ReadFromFile(string path)
    {
        await using var openStream = File.OpenRead(path);
        var widgets = await JsonSerializer.DeserializeAsync<List<Widget>>(openStream);
        return widgets;
    }

    static bool ProcessFile(string path)
    {
        try
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var task = ReadFromFile(path);
            long lastTrack = 0;
            while (!task.IsCompleted)
            {
                if (stopwatch.ElapsedMilliseconds % 500 != 0 || lastTrack >= stopwatch.ElapsedMilliseconds)
                    continue;
                Console.Clear();
                Success($"Reading file: {stopwatch.ElapsedMilliseconds} mls");
                lastTrack = stopwatch.ElapsedMilliseconds;
            }
            
            Manager.Widgets = task.Result;
        }
        catch (Exception e)
        {
            Warning($"Can't read data from your file. Error: {e.Message}");
            Warning("Press any key to try again");
            return false;
        }

        if (Manager.Widgets == null)
        {
            Warning($"Data in the file wasn't decoded.");
            Warning("Press any key to try again");
            return false;
        }

        Manager.FileName = path;
        return true;
    }
    
    static void Main()
    {
        Console.CursorVisible = false;
        List<Widget>? widgets;
        do
        {
            var value = SelectMethodMenu();
            if (value == 0)
            {
                Console.CursorVisible = true;
                do
                {
                    Header("Enter an absolute path to your file: ", false);
                    var path = Console.ReadLine();
                    if (path == null)
                    {
                        Warning("The input was interrupted by another process. Press any key to try again");
                        continue;
                    }

                    if (!ProcessFile(path)) continue;
                    break;
                } while (Console.ReadKey(true).KeyChar >= 0);

                Console.CursorVisible = false;
            }
            Header("Press Q to quit the app or any other key to continue...");
        } while (Console.ReadKey(true).Key != ConsoleKey.Q);
    }
}