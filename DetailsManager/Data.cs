namespace DetailsManager;

public struct Data 
{
    public List<Widget> Details;
    public AutoSaver AutoSaver;
    public string FileName;

    public Data(string fileName)
    {
        FileName = fileName;
        Details = new List<Widget>();
        AutoSaver = new AutoSaver();
    }
}