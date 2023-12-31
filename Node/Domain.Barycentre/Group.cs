﻿using System.Collections.Immutable;
using Domain.Common.Seedwork.Abstract;
using Domain.Publicity;

namespace Domain.Grouping;

public sealed class Group : Aggregate<Group>
{
    public double Barycentre { get; set; }

    public ImmutableList<Space> Spaces { get; set; }

    public Group(string id, double barycentre, ImmutableList<Space> spaces) : base(id)
    {
        Barycentre = barycentre;
        Spaces = spaces;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Barycentre: {Barycentre}";
    }
}