using System.Collections.Immutable;
using Domain.Common.Seedwork.Abstract;

namespace Domain.Grouping;

public sealed class Group : Aggregate<Group>
{
    public double Barycentre { get; }

    public ImmutableList<string> Spaces { get; }

    public Group(string id, double barycentre, ImmutableList<string> spaces) : base(id)
    {
        Barycentre = barycentre;
        Spaces = spaces;
    }
}