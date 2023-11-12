using Application.Common.Interfaces;

namespace Configuration;

internal sealed class HostInfo : IHostInfo
{
    private const string BrokerPortName = "BROKER_PORT";

    private static readonly int BrokerPortEnv = Convert.ToInt32(Environment.GetEnvironmentVariable(BrokerPortName) ?? throw new Exception($"{BrokerPortName} env variable is not set"));

    private const string MasterName = "MASTER";

    private static readonly bool IsMasterEnv = Convert.ToBoolean(Environment.GetEnvironmentVariable(MasterName) ?? "false");

    private const string NbOfIterationName = "NB_OF_ITERATION";

    private static readonly int NbOfIterationEnv = Convert.ToInt32(Environment.GetEnvironmentVariable(NbOfIterationName) ?? "1");

    private const string NodeTypeName = "TYPE";

    private static readonly string NodeTypeEnv = Environment.GetEnvironmentVariable(NodeTypeName) ?? throw new Exception($"{NodeTypeName} env variable is not set");

    private const string ListenTopicsName = "LISTEN_TOPICS";

    private static readonly string ListenTopicsEnv = Environment.GetEnvironmentVariable(ListenTopicsName) ?? "";

    private const string PublishTopicsName = "PUBLISH_TOPICS";

    private static readonly string PublishTopicsEnv = Environment.GetEnvironmentVariable(PublishTopicsName) ?? "";

    public string Host => "host.docker.internal";

    public int BrokerPort => BrokerPortEnv;

    public bool IsMaster => IsMasterEnv;

    public string NodeType => NodeTypeEnv;

    public string ListenTopics => ListenTopicsEnv;

    public string PublishTopics => PublishTopicsEnv;

    public int NbOfIteration => NbOfIterationEnv;

    public string DataCsvName { get; } = "data.csv";

    public string GroupCsvName { get; } = "groups.csv";
}