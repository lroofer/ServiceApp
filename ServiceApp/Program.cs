using static DetailsManager.Markup;

namespace ServiceApp;

internal static class Program
{
    static async Task<int> SelectMenuAsync()
    {
        var menu = new Menu(new []
        {
            "Enter an absolute path to the file [recommended]",
            "Use FileManager to select a file"
        }, "[DetailsManager]");
        return await Task.Run(() => menu.Run());
    } 
    static void Main()
    {
        Console.CursorVisible = false;
        do
        {
            var value = SelectMenuAsync().Result;
            Console.WriteLine(value);
            Header("Press Q to quit the app or any other key to continue...");
        } while (Console.ReadKey(true).Key != ConsoleKey.Q);
    }
}