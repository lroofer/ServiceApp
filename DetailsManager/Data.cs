namespace DetailsManager;

public class Data
{
    public WidgetList WidgetList;
    public AutoSaver AutoSaver;
    public string FileName;

    public Data(string fileName)
    {
        WidgetList = new WidgetList();
        FileName = fileName;
        AutoSaver = new AutoSaver();
    }
}