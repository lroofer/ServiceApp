using System.Text.Json;

namespace DetailsManager;

public class AutoSaver
{
    private DateTime _lastApplied;

    private async void OnUpdate(object? sender, UpdateArgs e)
    {
        var diff = (e.TimeReached - _lastApplied).TotalSeconds;
        if (!(diff >= 15)) return;
        await using var createStream = File.Create(Manager.TempFileName ?? "data_tmp.json");
        await JsonSerializer.SerializeAsync(createStream, Manager.Widgets);
        _lastApplied = e.TimeReached;
    }
    public AutoSaver()
    {
        _lastApplied = DateTime.Now;
        Widget.Updated += OnUpdate;
        Specification.Updated += OnUpdate;
    }
}