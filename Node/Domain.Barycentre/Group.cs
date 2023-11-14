using System.Collections.Immutable;
using System.Xml.Schema;
using Domain.Common.Seedwork.Abstract;
using Domain.Publicity;

namespace Domain.Grouping;

public sealed class Group : Aggregate<Group>
{
    public double Barycentre { get; }

    public ImmutableList<Space> Spaces { get; }

    public Group(string id, double barycentre, ImmutableList<Space> spaces) : base(id)
    {
        Barycentre = barycentre;
        Spaces = spaces;
    }
}