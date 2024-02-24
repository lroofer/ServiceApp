using DetailsManager;

using static DetailsManager.Markup;

namespace ServiceApp;

public class ObjectView: View<IOption>
{
    private IDisplayable _object;

    public ObjectView(IDisplayable obj, string objectName): base(obj.GetOptions(), $"Manage {objectName} object")
    {
        _object = obj;
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
                case ConsoleKey.Enter:
                    Console.ResetColor();
                    if (!Options[SelectedOption].IsMutable()) break;
                    Options[SelectedOption].Expand(_object);
                    Init();
                    break;
            }
        } while (key != ConsoleKey.X);
        Console.ResetColor();
        Console.Clear();
    }
}