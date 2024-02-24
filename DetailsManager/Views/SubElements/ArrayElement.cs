using DetailsManager.Objects;
using DetailsManager.Protocols;
using static DetailsManager.Markup;

namespace DetailsManager.Views.SubElements;

/// <summary>
/// Interactive option that can be displayed as a fully independent view.
/// </summary>
public class ArrayElement : View<Specification>, IOption
{
    private readonly string _tag;

    public ArrayElement(string tag, in List<Specification> lst) : base(lst, "Manage list of specifications")
    {
        _tag = tag;
    }

    public bool IsMutable()
        => true;

    public string GetTag()
        => _tag;

    /// <summary>
    /// Displays the array as a separate view.
    /// </summary>
    /// <param name="object">The object to sync with.</param>
    public void Expand(IDisplayable @object)
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
                        var specificationView = new ObjectView(Options[SelectedOption], "Specification");
                        specificationView.Run();
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

    /// <summary>
    /// Creates a snippet of the object.
    /// </summary>
    /// <returns>Presented info</returns>
    public override string ToString() => $"{_tag}: {Options.Count} inst.";
}