namespace Application.Common.Interfaces;

public interface IHostInfo
{
    string DataCsvName { get; }

    string GroupCsvName { get; }

    string Host { get; }
    
    int BrokerPort { get; }
    
    bool IsMaster { get; }
    
    string NodeType { get; }
    
    string ListenTopics { get; }
    
    string PublishTopics { get; }
    
    int NbOfIteration { get; }
}