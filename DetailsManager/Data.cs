namespace DetailsManager;

public struct Data 
{
    public List<Widget> Details;
    public List<Specification> Specifications;
    public AutoSaver AutoSaver;
    public string FileName;

    public Data(string fileName)
    {
        FileName = fileName;
        Details = new List<Widget>();
        Specifications = new List<Specification>();
        AutoSaver = new AutoSaver();
    }
}