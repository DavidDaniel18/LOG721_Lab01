using Domain.Common;

namespace Application.Common.Interfaces;

public interface IPublisher<in TContract> where TContract : class
{
    Task<Result> Publish(TContract payload, string routingKey);
}