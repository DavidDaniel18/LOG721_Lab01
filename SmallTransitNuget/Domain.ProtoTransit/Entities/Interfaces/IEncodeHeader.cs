using Domain.Common;

namespace Domain.ProtoTransit.Entities.Interfaces;

public interface IEncodeHeader
{
    Result<byte[]> GetBytes();
}