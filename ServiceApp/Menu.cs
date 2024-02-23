namespace ServiceApp;

using static DetailsManager.Markup;

public class Menu
{
    private const int OptionLength = 60;
    private int _selectedOption;
    private readonly string[] _options;
    private readonly int[] _optionLocations;
    private readonly string _prompt;

    public Menu() : this(Array.Empty<string>(), "")
    {
    }
    public Menu(string[] options, string prompt, int selectedOption = 0)
    {
        _selectedOption = selectedOption;
        _options = options;
        if (options.Any(option => option.Contains('\n') || option.Length > OptionLength))
        {
            throw new ArgumentException("Options must be shorter");
        }
        _prompt = prompt;
        _optionLocations = new int[_options.Length];
    }

    private void Init()
    {
        Console.Clear();
        Header(_prompt);
        for (var i = 0; i < _options.Length; ++i)
        {
            Console.ForegroundColor = i == _selectedOption ? BackgroundColor : ForegroundColor;
            Console.BackgroundColor = i == _selectedOption ? ForegroundColor : BackgroundColor;
            _optionLocations[i] = Console.GetCursorPosition().Top;
            Console.WriteLine($"<{(i == _selectedOption ? '*' : ' ')}> {_options[i]}");
        }
    }

    private void Down()
    {
        Console.SetCursorPosition(0, _optionLocations[_selectedOption]);
        Console.ForegroundColor = ForegroundColor;
        Console.BackgroundColor = BackgroundColor;
        Console.WriteLine($"<-> {_options[_selectedOption++]}");
        Console.BackgroundColor = ForegroundColor;
        Console.ForegroundColor = BackgroundColor;
        Console.WriteLine($"<*> {_options[_selectedOption]}");
    }

    private void Up()
    {
        Console.SetCursorPosition(0, _optionLocations[--_selectedOption]);
        Console.ForegroundColor = BackgroundColor;
        Console.BackgroundColor = ForegroundColor;
        Console.WriteLine($"<*> {_options[_selectedOption]}");
        Console.BackgroundColor = BackgroundColor;
        Console.ForegroundColor = ForegroundColor;
        Console.WriteLine($"<-> {_options[_selectedOption + 1]}");
    }
    
    public int Run()
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
                case ConsoleKey.DownArrow when _selectedOption != _options.Length - 1:
                    Down();
                    break;
            }
        } while (key != ConsoleKey.Enter);
        Console.ResetColor();
        Console.Clear();
        return _selectedOption;
    }
}