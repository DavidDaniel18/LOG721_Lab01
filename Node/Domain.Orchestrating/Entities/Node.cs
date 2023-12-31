﻿using Domain.Common.Seedwork.Abstract;

namespace Domain.Orchestrating.Entities;

internal sealed class Node : Entity<Node>
{
    public NodeType NodeType { get; set; }
    public bool IsMaster { get; set; }

    public Node(string id) : base(id)
    {
    }
}