﻿using Microsoft.Extensions.DependencyInjection;
using SmallTransit.Abstractions.Interfaces;
using SmallTransit.Domain.Services.Receiving;

namespace SmallTransit.Dispatcher;

public sealed class ControllerDispatcher<TContract> : IControllerDelegate<TContract> where TContract : class
{
    private readonly IServiceProvider _serviceProvider;

    public ControllerDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendToController(TContract contract)
    {
        var handler = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IConsumer<TContract>>();

        await handler.Consume(contract);
    }
}