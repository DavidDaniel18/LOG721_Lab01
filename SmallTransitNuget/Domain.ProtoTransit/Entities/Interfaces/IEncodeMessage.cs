using Domain.Common;

namespace Domain.ProtoTransit.Entities.Interfaces;

public interface IEncodeMessage
{
    Result<byte[]> GetBytes();
}