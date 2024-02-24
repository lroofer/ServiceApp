namespace DetailsManager;

using static Markup;

public class BoolElement: IOption
{
    private string _tag;

    public BoolElement(string tag, bool value)
    {
        _tag = tag;
        Value = value;
    }
    
    public bool Value { get; private set; }
    
    public bool IsMutable()
        => true;

    public string GetTag()
        => _tag;

    public override string ToString()
        => $"{_tag}: {(Value ? "V": "X")}";

    public void Expand(IDisplayable @object)
    {
        Console.Clear();
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
    }
}