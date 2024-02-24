namespace DetailsManager;

public class ArrayElement: IOption
{
    private readonly string _tag;
    private List<Specification> _specifications;
    
    public ArrayElement(string tag, in List<Specification> lst)
    {
        _tag = tag;
        _specifications = lst;
    }
    
    public bool IsExpandable()
        => true;

    public bool IsMutable()
        => false;

    public void SetValue(string value)
    {
        throw new NotImplementedException();
    }

    public string GetTag()
        => _tag;

    public void Expand()
    {
        throw new NotImplementedException();
    }
    
    public override string ToString() => $"{_tag}: {_specifications.Count} inst.";

}