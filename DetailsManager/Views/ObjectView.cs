using DetailsManager.Objects;
using DetailsManager.Protocols;

namespace DetailsManager.Views;

using static Markup;

/// <summary>
/// The view of any IDisplayable object.
/// </summary>
public class ObjectView : View<IOption>
{
    private readonly IDisplayable _object;

    public ObjectView(IDisplayable obj = null!, string objectName = "specification") : base(obj.GetOptions(), $"Manage {objectName} object")
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
                    try
                    {
                        if (!Options[SelectedOption].IsMutable()) break;
                        Options[SelectedOption].Expand(_object);
                        Options = _object.GetOptions();
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
        } while (key != ConsoleKey.X);

        Console.ResetColor();
        Console.Clear();
    }
}