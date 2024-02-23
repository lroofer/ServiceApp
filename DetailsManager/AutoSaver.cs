namespace DetailsManager;

public class AutoSaver
{
    private DateTime _lastApplied;

    private void OnUpdate(object? sender, UpdateArgs e)
    {
        var diff = (e.TimeReached - DateTime.Now).TotalSeconds;
        if (diff >= 15)
        {
            //var jsonValue = Manager.Widgets? ?? "{}";
            //var fileName = $"{Manager.Data?.FileName ?? ""}_tmp.json";
        }
    }
    public AutoSaver()
    {
        _lastApplied = DateTime.Now;
        Widget.Updated += OnUpdate;
        Specification.Updated += OnUpdate;
    }
}