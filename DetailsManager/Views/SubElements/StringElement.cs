using DetailsManager.Protocols;

namespace DetailsManager.Views.SubElements;

using static Markup;

/// <summary>
/// The representation of the string property.
/// </summary>
public class StringElement : IOption
{
    private readonly bool _isMutable;

    private readonly string _tag;

    public StringElement(string tag = "", string value = "", bool isMutable = false)
    {
        _tag = tag;
        Value = value;
        _isMutable = isMutable;
    }

    public bool IsMutable() => _isMutable;
    public string Value { get; private set; }

    public string GetTag() => _tag;

    public override string ToString() => $"{_tag}: {Value}";

    public void Expand(IDisplayable @object)
    {
        Console.CursorVisible = true;
        Console.Clear();
        var oldValue = Value;
        Header($"Change mutable value {_tag}: {oldValue}");
        while (true)
        {
            Console.Write("New value: ");
            var tr = Console.ReadLine();
            if (tr == null)
            {
                Warning("Input was interrupted by another process. Try again: ");
                continue;
            }

            Value = tr;
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