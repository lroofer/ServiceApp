using System.Diagnostics;
using System.Text.Json;
using DetailsManager;
using DetailsManager.Objects;
using DetailsManager.Views;
using static DetailsManager.Markup;

namespace ServiceApp;

internal static class Program
{
    static int SelectMethodMenu()
    {
        var menu = new Menu(new []
        {
            "Enter an absolute path to the file [recommended]"
            //"Use FileManager to select a file"
        }, "[DetailsManager]");
        return menu.Run();
    }
    
    static async Task<(bool, string)> ProcessFile(string path)
    {
        string? tempFileName;
        try
        {
            if (Path.GetExtension(path) != ".json")
            {
                return (false, "Your file must have json extension");
            }

            tempFileName = Path.GetFileNameWithoutExtension(path) + "_tmp.json";
            await using var openStream = File.OpenRead(path);
            Manager.Widgets = await JsonSerializer.DeserializeAsync<List<Widget>>(openStream);
        }
        catch (Exception e)
        {
            var msg = $"Can't read data from your file. Error: {e.Message}\nPress any key to try again";
            return (false, msg);
        }

        if (Manager.Widgets == null)
        {
            const string msg = $"Data in the file wasn't decoded. Press any key to try again";
            return (false, msg);
        }
        
        Manager.AutoSaver = new AutoSaver();
        Manager.PathFile = path;
        Manager.TempFileName = tempFileName;
        return (true, "Done");
    }
    
    static void Main()
    {
        Console.CursorVisible = false;
        do
        {
            try
            {
                Console.Clear();
                Header(
                    "[To ensure great performance and correct displaying of data please EXPAND your terminal window]");
                Success("By pressing enter you admit that window size won't be changed by yourself");
                Console.ReadLine();
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

                        var stopwatch = new Stopwatch();
                        stopwatch.Start();
                        var task = ProcessFile(path);
                        long lastTrack = 0;
                        while (!task.IsCompleted)
                        {
                            if (stopwatch.ElapsedMilliseconds % 500 != 0 || lastTrack >= stopwatch.ElapsedMilliseconds)
                                continue;
                            Console.Clear();
                            Success($"Processing file: {stopwatch.ElapsedMilliseconds} mls");
                            lastTrack = stopwatch.ElapsedMilliseconds;
                        }

                        if (task.Result.Item1) break;
                        Warning(task.Result.Item2);
                    } while (Console.ReadKey(true).KeyChar >= 0);

                    Console.CursorVisible = false;
                }
                else
                {
                    // MARK: File manager logic here.
                }

                var widgetView = new WidgetsView();
                widgetView.Run();
            }
            catch (Exception e)
            {
                Warning($"Something went wrong: {e.Message}");
            }

            Header("Press Q to quit the app or any other key to continue...");
        } while (Console.ReadKey(true).Key != ConsoleKey.Q);
    }
}