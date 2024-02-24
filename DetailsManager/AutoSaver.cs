using System.Diagnostics;
using System.Text.Json;

namespace DetailsManager;

public class AutoSaver
{
    private DateTime _lastApplied;

    private async void OnUpdate(object? sender, UpdateArgs e)
    {
        var diff = (e.TimeReached - _lastApplied).TotalSeconds;
        if (!(diff >= 15)) return;
        try
        {
            await using var createStream = File.Create(Manager.TempFileName ?? "data_tmp.json");
            await JsonSerializer.SerializeAsync(createStream, Manager.Widgets);
            _lastApplied = e.TimeReached;
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Unable to write a file {exception.Message}");
        }
    }
    public AutoSaver()
    {
        _lastApplied = DateTime.Now;
        Widget.Updated += OnUpdate;
        Specification.Updated += OnUpdate;
    }
}