using Microsoft.Extensions.DependencyInjection;
using SmallTransit.Abstractions.Interfaces;
using SmallTransit.Abstractions.Receiver;
using SmallTransit.Domain.ProtoTransit.Extensions;
using SmallTransit.Domain.Services.Receiving;
using SmallTransit.Domain.Services.Sending.Subscribing.Dto;
using SmallTransit.Presentation.Controllers.Dto.Configurator;

namespace SmallTransit.Dispatcher;

public sealed class ReceiveControllerDispatcher : IReceiveControllerDelegate
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Configurator _configurator;

    public ReceiveControllerDispatcher(IServiceProvider serviceProvider, Configurator configurator)
    {
        _serviceProvider = serviceProvider;
        _configurator = configurator;
    }

    public (Type Contract, Type ReturnType) GetContractType(string contractName)
    {
        var configuration = _configurator.ReceiverPointConfigurators
            .FirstOrDefault(configuration => configuration.ContractType.GetTypeName().Equals(contractName, StringComparison.InvariantCultureIgnoreCase));

        return configuration is null
            ? throw new InvalidOperationException($"Configuration for contract {contractName} not found")
            : (configuration.ContractType, configuration.ResultType);
    }

    public async Task<object> SendToController(ReceiveWrapper wrapper)
    {
        using var scope = _serviceProvider.CreateScope();

        var genericTypeDefinition = typeof(Dispatcher<,>);

        var closedGenericType = genericTypeDefinition.MakeGenericType(wrapper.ContractType, wrapper.ResultType);

        var dispatcher = (IDispatcher)Activator.CreateInstance(closedGenericType)!;

        return await dispatcher.SendToController(wrapper, scope.ServiceProvider);
    }

    private interface IDispatcher
    {
        Task<object> SendToController(ReceiveWrapper wrapper, IServiceProvider serviceProvider);
    }

    private class Dispatcher<TContract, TResult> : IDispatcher
    where TContract : class
    where TResult : notnull
    {
        public async Task<object> SendToController(ReceiveWrapper wrapper, IServiceProvider serviceProvider)
        {
            if (wrapper.Payload.GetType() != typeof(TContract))
            {
                throw new InvalidOperationException($"Payload type {wrapper.Payload.GetType()} is not equal to {typeof(TContract)}");
            }

            var context = new ReceiveContext<TContract>(wrapper.SenderId, (TContract)wrapper.Payload);

            var receiver = serviceProvider.GetRequiredService<IReceiver<TContract, TResult>>();

            return await receiver.Consume(context);
        }
    }
}