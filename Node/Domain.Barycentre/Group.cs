using System.Collections.Immutable;
using Domain.Common.Seedwork.Abstract;
using Domain.Publicity;

namespace Domain.Grouping;

public sealed class Group : Aggregate<Group>
{
    public double Barycentre { get; }

    public List<Space> Spaces { get; }

    public Group(string id, double barycentre, List<Space> spaces) : base(id)
    {
        Barycentre = barycentre;
        Spaces = spaces;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Barycentre: {Barycentre}, NbOfSpaces: {Spaces.Count()}";
    }
}