using Domain.Common.Seedwork.Abstract;

namespace Domain.Orchestrating;

public sealed class Orchestrator : Aggregate<Orchestrator>
{
    public Orchestrator(string id) : base(id)
    {
    }
}