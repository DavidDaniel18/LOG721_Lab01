using Application.Services.InfrastructureInterfaces;
using Domain.Common;
using Domain.ProtoTransit;
using Domain.ProtoTransit.ValueObjects.Properties;
using MessagePack;

namespace Application.Services;

internal sealed class Communication
{
    private readonly IRead _read;
    private readonly IWrite _write;

    internal Communication(IRead read, IWrite write)
    {
        _read = read;
        _write = write;
    }

    private async Task<Result> Publish<TContract>(TContract contract) where TContract : class
    {
        var handshake = await HandShake();

        if (handshake.IsFailure()) return Result.FromFailure(handshake);

        var publish = MessageFactory.Create(MessageTypesEnum.Publish);

        publish.TrySetProperty<Payload>(MessagePackSerializer.Serialize(contract));

        var serializedPublishMessage = publish.GetBytes();

        if (serializedPublishMessage.IsFailure()) return Result.FromFailure(serializedPublishMessage);

        return await Send(serializedPublishMessage.Content!);

    }

    private async Task<Result> HandShake()
    {
        var handshake = MessageFactory.Create(MessageTypesEnum.HandShake);

        var serializedHandShake = handshake.GetBytes();

        if (serializedHandShake.IsFailure()) return Result.FromFailure(serializedHandShake);

        return await Send(serializedHandShake.Content!);
    }

    private async Task<Result> Send(byte[] message)
    {
        await _write.WriteAsync(message);

        return await WaitForResponse();
    }

    private async Task<Result> WaitForResponse()
    {
        Result result;

        do
        {
            var response = await _read.ReadAsync();

            result = Protocol.TryParseMessage(response);

        } while (result.IsFailure());

        return Result.Success();
    }
}