namespace Configuration;

internal sealed class HostInfo
{
    private const string BrokerPortName = "BROKER_PORT";

    private static readonly int BrokerPortEnv = Convert.ToInt32(Environment.GetEnvironmentVariable(BrokerPortName) ?? throw new Exception($"{BrokerPortName} env variable is not set"));

    public string Host { get; } = "host.docker.internal";

    public int BrokerPort => BrokerPortEnv;
}