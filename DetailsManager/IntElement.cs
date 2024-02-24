namespace DetailsManager;

public class IntElement: IOption
{
    private readonly string _tag;

    public IntElement(string tag, int value)
    {
        _tag = tag;
        GetValue = value;
    }

    public int GetValue { get; private set; }
    
    public bool IsExpandable() => false;

    public bool IsMutable() => true;

    public void SetValue(string value)
    {
        if (value.Length > 10) throw new ArgumentException("Must be shorter.");
        if (!int.TryParse(value, out var nVal))
        {
            throw new ArgumentException("Value must be int.");
        }

        GetValue = nVal;
    }

    public string GetTag() => _tag;
    
    public void Expand() => throw new InvalidOperationException();

    public override string ToString() => $"{_tag}: {GetValue}";
}