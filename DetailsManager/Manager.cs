namespace DetailsManager;

using static Markup;

public static class Manager
{
    public static List<Widget>? Widgets;
    public static AutoSaver? AutoSaver;
    public static string? PathFile;
    public static string? TempFileName;

    public delegate int CompareTo(Widget a, Widget b);
    public static void Sort(CompareTo ct)
    {
        Console.Clear();
        Header("Do you want to sort ascending? (Y/N or Q - go back)");
        while (true)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Y:
                    Widgets?.Sort((widget, widget1) => ct(widget, widget1));
                    return;
                case ConsoleKey.N:
                    Widgets?.Sort((widget, widget1) => ct(widget1, widget));
                    return;
                case ConsoleKey.Q:
                    return;
            }
        }
    }
}