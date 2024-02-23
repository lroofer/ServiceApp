using System.Text.Json;

namespace DetailsManager;

public class WidgetList
{
    public List<Widget> List { get; }

    public Widget this[int index]
    {
        get => List[index];
        set => List[index] = value;
    }

    public WidgetList()
    {
        List = new();
    }

    public WidgetList(List<Widget> widgets)
    {
        List = widgets;
    }
    public string ToJSON()
    {
        return JsonSerializer.Serialize(this);
    }
}