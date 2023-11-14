using System.Globalization;
using Application.Commands.Interfaces;
using Application.Common.Interfaces;
using Application.Dtos;
using CsvHelper;
using Infrastructure.FileHandlers.Interfaces;

namespace Infrastructure.FileHandlers;

public sealed class CsvHandler : ICsvHandler
{
    private readonly IDataReader _dataReader;
    private readonly IHostInfo _hostInfo;

    internal CsvHandler(IDataReader dataReader, IHostInfo hostInfo)
    {
        _dataReader = dataReader;
        _hostInfo = hostInfo;
    }

    public IAsyncEnumerable<DataDto> ReadDatasAsync()
    {
        return ReadAsync<DataDto>(_dataReader.GetString(_hostInfo.DataCsvName));
    }

    public IAsyncEnumerable<GroupDto> ReadGroupsAsync()
    {
        return ReadAsync<GroupDto>(_dataReader.GetString(_hostInfo.GroupCsvName));
    }

    public IEnumerable<DataDto> ReadDatas()
    {
        return Read<DataDto>(_dataReader.GetString(_hostInfo.GroupCsvName));
    }

    public IEnumerable<GroupDto> ReadGroups()
    {
        return Read<GroupDto>(_dataReader.GetString(_hostInfo.GroupCsvName));
    }

    private static async IAsyncEnumerable<TResult> ReadAsync<TResult>(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        await foreach (var record in csv.GetRecordsAsync<TResult>())
        {
            yield return record;
        }
    }

    private static IEnumerable<TResult> Read<TResult>(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        return csv.GetRecords<TResult>();
    }
}