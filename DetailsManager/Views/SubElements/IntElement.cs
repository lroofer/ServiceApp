using DetailsManager.Protocols;

namespace DetailsManager.Views.SubElements;

using static Markup;

/// <summary>
/// The representation of integer property.
/// </summary>
public class IntElement : IOption
{
    private readonly string _tag;

    public IntElement(string tag, int value)
    {
        _tag = tag;
        Value = value;
    }

    public int Value { get; private set; }

    public bool IsMutable() => true;

    public string GetTag() => _tag;

    /// <summary>
    /// Shorten representation of the property.
    /// </summary>
    /// <returns></returns>
    public override string ToString() => $"{_tag}: {Value}";

    /// <summary>
    /// Simple representation of integer changing logic.
    /// </summary>
    /// <param name="object">Object to sync with.</param>
    public void Expand(IDisplayable @object)
    {
        Console.Clear();
        var oldValue = Value;
        Header($"Change mutable value {_tag}: {oldValue}");
        while (true)
        {
            Console.Write("New value (int): ");
            var value = Console.ReadLine();
            if (value == null)
            {
                Warning("Input was interrupted by another process. Try again: ");
                continue;
            }

            if (!int.TryParse(value, out var nVal))
            {
                Warning("Value must be int.");
                continue;
            }

            Value = nVal;
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
    }
}