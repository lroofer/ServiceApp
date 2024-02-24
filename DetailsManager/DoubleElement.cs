namespace DetailsManager;

using static Markup;

public class DoubleElement: IOption
{
    private bool _isMutable;
    private string _tag;

    public DoubleElement(string tag, double value, bool isMutable)
    {
        _tag = tag;
        _isMutable = isMutable;
        Value = value;
    }

    public double Value { get; private set; }
    
    public bool IsMutable() => _isMutable;

    public string GetTag()
        => _tag;

    public void Expand(IDisplayable @object)
    {
        Console.Clear();
        var oldValue = Value;
        Header($"Change mutable value {_tag}: {oldValue}");
        while (true)
        {
            Console.Write("New value (double): ");
            var value = Console.ReadLine();
            if (value == null)
            {
                Warning("Input was interrupted by another process. Try again: ");
                continue;
            }
            if (!double.TryParse(value, out var nVal))
            {
                Warning("Value must be double.");
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
    
    public override string ToString() => $"{_tag}: {Math.Round(Value, 2)}";

}