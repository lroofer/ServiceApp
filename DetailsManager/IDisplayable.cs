namespace DetailsManager;

public interface IDisplayable
{
    public List<IOption> GetOptions();
    public void SetOption(IOption val);
}