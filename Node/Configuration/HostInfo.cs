using Application.Common.Interfaces;

namespace Node;

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

    private const string MapRoutingKeysName = "MAP_ROUTING_KEYS";

    private static readonly string MapRoutingKeysEnv = Environment.GetEnvironmentVariable(MapRoutingKeysName) ?? "";

    private const string ReduceRouginKeysName = "REDUCE_ROUTING_KEYS";

    private static readonly string ReduceRoutingKeysEnv = Environment.GetEnvironmentVariable(ReduceRouginKeysName) ?? "";

    private const string MapRoutingKeyName = "MAP_ROUTING_KEY";

    private static readonly string MapRoutingKeyEnv = Environment.GetEnvironmentVariable(MapRoutingKeyName) ?? "";

    private const string ReduceRoutingKeyName = "REDUCE_ROUTING_KEY";

    private static readonly string ReduceRoutingKeyEnv = Environment.GetEnvironmentVariable(ReduceRoutingKeyName) ?? "";

    private const string MapFinishedEventRoutingKeyName = "MAP_FINISHED_EVENT_ROUTING_KEY";

    private static readonly string MapFinishedEventRoutingKeyEnv = Environment.GetEnvironmentVariable(MapFinishedEventRoutingKeyName) ?? "";

    private const string ReduceFinishedEventRoutingKeyName = "REDUCE_FINISHED_EVENT_ROUTING_KEY";

    private static readonly string ReduceFinishedEventRoutingKeyEnv = Environment.GetEnvironmentVariable(ReduceFinishedEventRoutingKeyName) ?? "";

    private const string InputRoutingKeyName = "INPUT_ROUTING_KEY";

    private static readonly string InputRoutingKeyEnv = Environment.GetEnvironmentVariable(InputRoutingKeyName) ?? "";

    private const string MapShuffleRoutinKeyName = "MAP_SHUFFLE_ROUTING_KEY";

    private static readonly string MapShuffleRoutingKeyEnv = Environment.GetEnvironmentVariable(MapShuffleRoutinKeyName) ?? "";

    private const string SyncStorePairPortListName = "SYNC_STORE_PAIR_PORT_LIST";

    private static readonly List<int> SyncStorePairPortListEnv = (Environment.GetEnvironmentVariable(SyncStorePairPortListName) ?? "").Split(',').ToList().Select(p => Convert.ToInt32(p)).ToList();

    private const string SyncExposeName = "SYNC_EXPOSE";

    private static readonly int SyncExposeEnv = Convert.ToInt32(Environment.GetEnvironmentVariable(SyncExposeName) ?? "0");

    private const string NodeNameName = "NODE_NAME";

    private static readonly string NodeNameEnv = Environment.GetEnvironmentVariable(NodeNameName) ?? string.Empty;

    public string Host => "host.docker.internal";

    public int BrokerPort => BrokerPortEnv;

    public bool IsMaster => IsMasterEnv;

    public string NodeType => NodeTypeEnv;

    public string MapRoutingKey => MapRoutingKeyEnv;

    public string ReduceRoutingKey => ReduceRoutingKeyEnv;

    public string MapFinishedEventRoutingKey => MapFinishedEventRoutingKeyEnv;

    public string ReduceFinishedEventRoutingKey => ReduceFinishedEventRoutingKeyEnv;

    public string MapRoutingKeys => MapRoutingKeysEnv;

    public string ReduceRoutingKeys => ReduceRoutingKeysEnv;

    public string InputRoutingKey => InputRoutingKeyEnv;

    public string MapShuffleRoutingKey => MapShuffleRoutingKeyEnv;

    public int NbOfIteration => NbOfIterationEnv;

    public List<int> SyncStorePairPortList => SyncStorePairPortListEnv;

    public string DataCsvName { get; } = "data";

    public string GroupCsvName { get; } = "groups";

    public int SyncExpose => SyncExposeEnv;

    public string NodeName => NodeNameEnv;
}
