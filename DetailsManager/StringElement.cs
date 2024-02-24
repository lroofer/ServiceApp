namespace DetailsManager;

using static Markup;

public class StringElement: IOption
{
    private readonly bool _isMutable;

    private readonly string _tag;
    
    public bool IsExpandable() => false;

    public bool IsMutable() => _isMutable;
    public string GetValue { get; private set; }

    public string GetTag() => _tag;

    public void Expand(IDisplayable @object)
    {
        Console.Clear();
        var oldValue = GetValue;
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

            GetValue = tr;
            try
            {
                @object.SetOption(this);
                break;
            }
            catch (Exception e)
            {
                Warning(e.Message);
                GetValue = oldValue;
            }
        }
    }

    public StringElement(string tag, string value, bool isMutable)
    {
        _tag = tag;
        GetValue = value;
        _isMutable = isMutable;
    }
    
    public override string ToString() => $"{_tag}: {GetValue}";

}