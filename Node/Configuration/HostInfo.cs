using Application.Common.Interfaces;

namespace Configuration;

internal sealed class HostInfo : IHostInfo
{
    private const string BrokerPortName = "BROKER_PORT";

    private static readonly int BrokerPortEnv = Convert.ToInt32(Environment.GetEnvironmentVariable(BrokerPortName) ?? throw new Exception($"{BrokerPortName} env variable is not set"));

    public string Host => "host.docker.internal";

    public int BrokerPort => BrokerPortEnv;

    public string DataCsvName { get; } = "data.csv";

    public string GroupCsvName { get; } = "groups.csv";
}