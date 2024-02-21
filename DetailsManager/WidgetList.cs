using System.Text.Json;

namespace DetailsManager;

public class WidgetList
{
    public List<Widget> list;

    public Widget this[int index]
    {
        get => list[index];
        set => list[index] = value;
    }

    public WidgetList()
    {
        list = new();
    }

    public WidgetList(List<Widget> widgets)
    {
        list = widgets;
    }
    public string ToJSON()
    {
        return JsonSerializer.Serialize(this);
    }
}