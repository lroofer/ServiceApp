using DetailsManager;

using static DetailsManager.Markup;

namespace ServiceApp;

public class ObjectView
{
    private readonly string _objectName;
    private int _selectedOption;
    private readonly int[] _optionLocations;
    private readonly List<IOption> _options;
    private IDisplayable _object;

    public ObjectView(IDisplayable obj, string objectName)
    {
        _object = obj;
        _options = obj.GetOptions();
        _selectedOption = 0;
        _objectName = objectName;
        _optionLocations = new int[_options.Count];
    }

    private void Init(int startPosition = 0)
    {
        _selectedOption = startPosition;
        Console.Clear();
        Header($"Manage {_objectName} object");
        for (var i = 0; i < startPosition; ++i)
        {
            _optionLocations[i] = -1;
        }

        for (var i = startPosition; i < _options.Count; ++i)
        {
            if (i - startPosition >= Console.BufferHeight - 3)
            {
                _optionLocations[i] = -1;
                continue;
            }
            Console.ForegroundColor = i == _selectedOption ? BackgroundColor : ForegroundColor;
            Console.BackgroundColor = i == _selectedOption ? ForegroundColor : BackgroundColor;
            _optionLocations[i] = Console.GetCursorPosition().Top;
            Console.WriteLine($"<{(i == _selectedOption ? '*' : '-')}> {_options[i]}");
        }
        Console.SetCursorPosition(0, Console.BufferHeight - 2);
        Formula("Enter");
        Success(" - Expand || ", false);
        Formula("X");
        Success(" - Back || ", false);
        Formula("Q");
        Success(" - Exit ", false);
        Console.WriteLine();
    }
    
    private void Up()
    {
        if (_optionLocations[_selectedOption - 1] == -1)
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

            _selectedOption--;
            Init(Math.Max(0, _selectedOption - Console.BufferHeight + 2));
            return;
        }
        Console.SetCursorPosition(0, _optionLocations[--_selectedOption]);
        Console.ForegroundColor = BackgroundColor;
        Console.BackgroundColor = ForegroundColor;
        Console.WriteLine($"<*> {_options[_selectedOption]}");
        Console.BackgroundColor = BackgroundColor;
        Console.ForegroundColor = ForegroundColor;
        Console.WriteLine($"<-> {_options[_selectedOption + 1]}");
    }
    
    private void Down()
    {
        if (_optionLocations[_selectedOption + 1] == -1)
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
            Init(++_selectedOption);
            return;
        }
        Console.SetCursorPosition(0, _optionLocations[_selectedOption]);
        Console.ForegroundColor = ForegroundColor;
        Console.BackgroundColor = BackgroundColor;
        Console.WriteLine($"<-> {_options[_selectedOption++]}");
        Console.BackgroundColor = ForegroundColor;
        Console.ForegroundColor = BackgroundColor;
        Console.WriteLine($"<*> {_options[_selectedOption]}");
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
                case ConsoleKey.UpArrow when _selectedOption != 0:
                    Up();
                    break;
                case ConsoleKey.DownArrow when _selectedOption != _options.Count - 1:
                    Down();
                    break;
                case ConsoleKey.B:
                    Console.ResetColor();
                    return;
                case ConsoleKey.Enter:
                    Console.ResetColor();
                    // Expand feature
                    break;
            }
        } while (key != ConsoleKey.Q);
        Console.ResetColor();
        Console.Clear();
    }
}