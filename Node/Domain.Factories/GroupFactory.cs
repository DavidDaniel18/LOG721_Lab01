using Domain.Grouping;
using Domain.Publicity;
using System.Collections.Immutable;

namespace Domain.Factories;

internal sealed class GroupFactory
{
    internal static Group Create(string id, double value)
    {
        return new Group(id, value, new List<Space>());
    }
}