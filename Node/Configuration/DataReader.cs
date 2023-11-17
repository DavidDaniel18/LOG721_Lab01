using System.Resources;
using Infrastructure.FileHandlers.Interfaces;

namespace ConfigurationNode;

public class DataReader : IDataReader
{
    private readonly ResourceManager _manager;
    private readonly ILogger<IDataReader> _logger;

    public DataReader(ILogger<IDataReader> logger)
    {
        _logger = logger;
        _manager = new ResourceManager("ConfigurationNode.Properties.Resources", typeof(Program).Assembly);
    }

    public string GetString(string key)
    {
        var data = _manager.GetString(key);

        if (string.IsNullOrWhiteSpace(data)) throw new Exception($"Data not found for key {key}");

        return data;
    }

    public object GetObject(string key)
    {
        var data = _manager.GetObject(key);

        if (data is null) throw new Exception($"Data not found for key {key}");

        return data;
    }
}