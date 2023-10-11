using Domain.Common;

namespace SmallTransit.Abstractions.Interfaces;

public interface IPublisher<in TContract> where TContract : class
{
    Task<Result> Publish(TContract payload, string routingKey);
}