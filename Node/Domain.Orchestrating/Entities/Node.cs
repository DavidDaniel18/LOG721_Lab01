using Domain.Common.Seedwork.Abstract;

namespace Domain.Orchestrating.Entities;

internal sealed class Node : Entity<Node>
{
    public Node(string id) : base(id)
    {
    }
}