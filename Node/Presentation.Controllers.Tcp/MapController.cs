﻿using Application.Commands.Map.Mapping;
using Application.Commands.Seedwork;
using Domain.Publicity;
using SmallTransit.Abstractions.Interfaces;

namespace Presentation.Controllers.Tcp;

public class MapController : IConsumer<MapCommand>
{
    private readonly ICommandHandler<MapCommand> _handler;
    
    public MapController(ICommandHandler<MapCommand> handler)
    {
        _handler = handler;
    }

    public Task Consume(MapCommand contract)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        _handler.Handle(contract, cancellationTokenSource.Token);

        return Task.CompletedTask;
    }
}