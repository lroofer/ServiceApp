using DetailsManager.Protocols;

namespace DetailsManager.Views.SubElements;

using static Markup;

/// <summary>
/// The representation of boolean property.
/// </summary>
public class BoolElement : IOption
{
    private readonly string _tag;

    public BoolElement(string tag = "", bool value = false)
    {
        _tag = tag;
        Value = value;
    }

    public bool Value { get; private set; }

    public bool IsMutable()
        => true;

    public string GetTag()
        => _tag;

    /// <summary>
    /// Shorten representation of the property.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => $"{_tag}: {(Value ? "V" : "X")}";

    /// <summary>
    /// Simple representation of the boolean changing logic.
    /// </summary>
    /// <param name="object">Object to sync with.</param>
    public void Expand(IDisplayable @object)
    {
        Console.Clear();
        Console.CursorVisible = true;
        var oldValue = Value;
        Header($"Change mutable value {_tag}: {oldValue}");
        while (true)
        {
            Console.Write("New value(true/false): ");
            var tr = Console.ReadLine();
            if (tr == null)
            {
                Warning("Input was interrupted by another process. Try again: ");
                continue;
            }

            tr = tr.ToLower();
            if (tr is not ("true" or "false"))
            {
                Warning("Wrong value: try true/false");
                continue;
            }

            Value = tr == "true";
            try
            {
                @object.SetOption(this);
                break;
            }
            catch (Exception e)
            {
                Warning(e.Message);
                Value = oldValue;
            }
        }
        Console.CursorVisible = false;
    }
}