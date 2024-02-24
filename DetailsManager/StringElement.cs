namespace DetailsManager;

public class StringElement: IOption
{
    private readonly bool _isMutable;

    private readonly string _tag;
    
    public bool IsExpandable() => false;

    public bool IsMutable() => _isMutable;

    public void SetValue(string value)
        => GetValue = value;
    public string GetValue { get; private set; }

    public string GetTag() => _tag;
    
    public void Expand() => throw new InvalidOperationException();

    public StringElement(string tag, string value, bool isMutable)
    {
        _tag = tag;
        GetValue = value;
        _isMutable = isMutable;
    }
    
    public override string ToString() => $"{_tag}: {GetValue}";

}