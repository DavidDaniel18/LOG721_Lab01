using Domain.Publicity;

namespace Domain.Factories;

internal sealed class PublicitySpaceFactory
{
    internal static Space Create(int width, int price)
    {
        return new Space(Guid.NewGuid().ToString(), width, price);
    }

    internal static Space Create(string id, int width, int price)
    {
        return new Space(id, width, price);
    }
}