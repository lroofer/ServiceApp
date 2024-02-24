namespace DetailsManager.Views;

/// <summary>
/// The view for the option selection.
/// </summary>
public class Menu: View<string>
{
    private const int OptionLength = 60;
    
    public Menu(string[] options = null!, string prompt = "", int selectedOption = 0) : base(options.ToList(), prompt)
    {
        if (options.Any(option => option.Contains('\n') || option.Length > OptionLength))
        {
            throw new ArgumentException("Options must be shorter");
        }
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
                case ConsoleKey.UpArrow when SelectedOption != 0:
                    Up();
                    break;
                case ConsoleKey.DownArrow when SelectedOption != Options.Count - 1:
                    Down();
                    break;
            }
        } while (key != ConsoleKey.Enter);
        Console.ResetColor();
        Console.Clear();
        return SelectedOption;
    }
    
    /// <summary>
    /// Disable buttonsView entirely.
    /// </summary>
    protected override void ButtonsView()
    {
        
    }
    
}