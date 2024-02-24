using DetailsManager.Objects;

namespace DetailsManager.Views;

using static Markup;

/// <summary>
/// The view of widgets.
/// </summary>
public class WidgetsView : View<Widget>
{
    public WidgetsView() : base(Manager.Widgets ?? new List<Widget>(), "Online shelf for widgets")
    {
    }

    public void Run()
    {
        Init();
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow when SelectedOption != 0:
                    Up();
                    break;
                case ConsoleKey.DownArrow when SelectedOption != Options.Count - 1:
                    Down();
                    break;
                case ConsoleKey.S:
                    Console.ResetColor();
                    try
                    {
                        Sort();
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Warning($"There's been an error while sorting.\n{e.Message}\n Press enter to try again!");
                        Console.ReadLine();
                    }

                    Options = Manager.Widgets ?? new List<Widget>();
                    Init();
                    break;
                case ConsoleKey.E:
                    Console.ResetColor();
                    try
                    {
                        Export();
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Warning($"There's been an error while exporting.\n{e.Message}\n Press enter to try again!");
                        Console.ReadLine();
                    }

                    Init();
                    break;
                case ConsoleKey.Enter:
                    Console.ResetColor();
                    try
                    {
                        var widgetView = new ObjectView(Options[SelectedOption] ?? throw new IndexOutOfRangeException(),
                            "Widget");
                        widgetView.Run();
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        Warning($"The connection with the view was lost.\n{e.Message}\n Press enter to try again!");
                        Console.ReadLine();
                    }

                    Init();
                    break;
            }
        } while (key != ConsoleKey.Q);

        Console.ResetColor();
        Console.Clear();
    }

    protected override void ButtonsView()
    {
        Console.SetCursorPosition(0, Console.BufferHeight - 2);
        Formula("Enter");
        Success(" - Expand || ", false);
        Formula("S");
        Success(" - Sort || ", false);
        Formula("E");
        Success("- Export ||", false);
        Formula("Q");
        Success(" - Exit ", false);
    }

    /// <summary>
    /// Exports file
    /// </summary>
    private static void Export()
    {
        Console.Clear();
        Header($"Ready to export file, choose path: (default) {Manager.PathFile}");
        Console.CursorVisible = true;
        var name = Console.ReadLine() ?? Manager.PathFile;
        if (name is "") name = Manager.PathFile;
        try
        {
            Manager.AutoSaver?.WriteFile(name ?? "");
            Success($"Export was succeed: {name}. Press enter");
        }
        catch (Exception e)
        {
            Warning($"There's been a error with you file. \n{e.Message} \nPress enter to return.");
        }
        Console.ReadLine();
        Console.CursorVisible = false;
    }

    /// <summary>
    /// Collect data for sorting.
    /// </summary>
    private static void Sort()
    {
        var menu = new Menu(Widget.WidgetProperties, "Choose sorting property");
        switch (menu.Run())
        {
            case 0:
                Manager.Sort((a, b) => string.Compare(a.WidgetId, b.WidgetId, StringComparison.Ordinal));
                return;
            case 1:
                Manager.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
                return;
            case 2:
                Manager.Sort((a, b) => a.Quantity.CompareTo(b.Quantity));
                return;
            case 3:
                Manager.Sort((a, b) => a.Price.CompareTo(b.Price));
                return;
            case 4:
                Manager.Sort((a, b) => a.IsAvailable.CompareTo(b.IsAvailable));
                return;
            case 5:
                Manager.Sort((a, b) => string.Compare(a.ManufactureDate, b.ManufactureDate, StringComparison.Ordinal));
                return;
            default:
                return;
        }
    }
}