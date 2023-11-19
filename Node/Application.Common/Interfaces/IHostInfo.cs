namespace Application.Common.Interfaces;

public interface IHostInfo
{
    string DataCsvName { get; }

    string GroupCsvName { get; }

    string Host { get; }
    
    int BrokerPort { get; }
    
    bool IsMaster { get; }
    
    string NodeType { get; }
    
    string MapRoutingKey { get; }
    
    string ReduceRoutingKey { get; }

    int SyncExpose { get; }

    string MapFinishedEventRoutingKey { get; }

    string ReduceFinishedEventRoutingKey { get; }

    string MapRoutingKeys { get; }

    string MapShuffleRoutingKey { get; }

    string ReduceRoutingKeys { get; }

    string InputRoutingKey { get; }

    List<int> SyncStorePairPortList { get; }

    int NbOfIteration { get; }

    string NodeName { get; }
}