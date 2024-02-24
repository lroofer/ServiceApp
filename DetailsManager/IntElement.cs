namespace DetailsManager;

using static Markup;

public class IntElement: IOption
{
    private readonly string _tag;

    public IntElement(string tag, int value)
    {
        _tag = tag;
        Value = value;
    }

    public int Value { get; private set; }

    public bool IsMutable() => true;

    public void SetValue(string value)
    {
        if (value.Length > 10) throw new ArgumentException("Must be shorter.");
        if (!int.TryParse(value, out var nVal))
        {
            throw new ArgumentException("Value must be int.");
        }

        Value = nVal;
    }

    public string GetTag() => _tag;

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
    public override string ToString() => $"{_tag}: {Value}";
}