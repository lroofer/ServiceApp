using DetailsManager;

namespace ServiceApp;

using static Markup;

public class WidgetsView
{
    private int _selectedWidget;
    private int[] _widgetLocations;

    public WidgetsView()
    {
        _selectedWidget = 0;
        _widgetLocations = new int[Manager.Widgets?.Count ?? 0];
    }

    private void Init(int startPosition = 0)
    {
        _widgetLocations = new int[Manager.Widgets?.Count ?? 0];
        _selectedWidget = startPosition;
        Console.Clear();
        Header("Online widget's shelf");
        for (var i = 0; i < startPosition; ++i)
        {
            _widgetLocations[i] = -1;
        }
        for (var i = startPosition; i < (Manager.Widgets?.Count ?? 0); ++i)
        {
            if (i - startPosition >= Console.BufferHeight - 3)
            {
                _widgetLocations[i] = -1;
                continue;
            }
            Console.ForegroundColor = i == _selectedWidget ? BackgroundColor : ForegroundColor;
            Console.BackgroundColor = i == _selectedWidget ? ForegroundColor : BackgroundColor;
            _widgetLocations[i] = Console.GetCursorPosition().Top;
            Console.WriteLine($"<{(i == _selectedWidget ? '*' : '-')}> {Manager.Widgets?[i]}");
        }
        Console.SetCursorPosition(0, Console.BufferHeight - 2);
        Formula("Enter");
        Success(" - Expand || ", false);
        Formula("S");
        Success(" - Sort || ", false);
        Formula("Q");
        Success(" - Exit ", false);
        Console.WriteLine();
    }

    private void Up()
    {
        if (_widgetLocations[_selectedWidget - 1] == -1)
        {
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
            Success("Press ", false);
            Formula("M");
            Success(" button to see previous widgets", false);
            if (Console.ReadKey(true).Key != ConsoleKey.M)
            {
                Console.SetCursorPosition(0, Console.BufferHeight - 1);
                Console.ResetColor();
                Console.Write("                                      ");
                return;
            }

            _selectedWidget--;
            Init(Math.Max(0, _selectedWidget - Console.BufferHeight + 2));
            return;
        }
        Console.SetCursorPosition(0, _widgetLocations[--_selectedWidget]);
        Console.ForegroundColor = BackgroundColor;
        Console.BackgroundColor = ForegroundColor;
        Console.WriteLine($"<*> {Manager.Widgets?[_selectedWidget]}");
        Console.BackgroundColor = BackgroundColor;
        Console.ForegroundColor = ForegroundColor;
        Console.WriteLine($"<-> {Manager.Widgets?[_selectedWidget + 1]}");
    }

    private void Sort()
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
    
    private void Down()
    {
        if (_widgetLocations[_selectedWidget + 1] == -1)
        {
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
            Console.ResetColor();
            Success("Press ", false);
            Formula("M");
            Success(" button to see more widgets", false);
            if (Console.ReadKey(true).Key != ConsoleKey.M)
            {
                Console.SetCursorPosition(0, Console.BufferHeight - 1);
                Console.ResetColor();
                Console.Write("                                  ");
                return;
            }
            Init(++_selectedWidget);
            return;
        }
        Console.SetCursorPosition(0, _widgetLocations[_selectedWidget]);
        Console.ForegroundColor = ForegroundColor;
        Console.BackgroundColor = BackgroundColor;
        Console.WriteLine($"<-> {Manager.Widgets?[_selectedWidget++]}");
        Console.BackgroundColor = ForegroundColor;
        Console.ForegroundColor = BackgroundColor;
        Console.WriteLine($"<*> {Manager.Widgets?[_selectedWidget]}");
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
                case ConsoleKey.UpArrow when _selectedWidget != 0:
                    Up();
                    break;
                case ConsoleKey.DownArrow when _selectedWidget != Manager.Widgets?.Count - 1:
                    Down();
                    break;
                case ConsoleKey.S:
                    Console.ResetColor();
                    Sort();
                    Init();
                    break;
                case ConsoleKey.Enter:
                    var widgetView = new WidgetView();
                    
                    break;
            }
        } while (key != ConsoleKey.Q);
        Console.ResetColor();
        Console.Clear();
    }
}