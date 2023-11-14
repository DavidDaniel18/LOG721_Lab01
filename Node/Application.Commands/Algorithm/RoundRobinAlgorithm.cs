using Application.Commands.Orchestrator.Service;


namespace Application.Commands.Algorithm;

public class RoundRobinAlgorithm : IRoundRobinAlgorithm
{
    private readonly List<string> _elements;

    private readonly int _size;

    private int _roundRobinIndex = -1;

    public RoundRobinAlgorithm(List<string> elements)
    {
        _elements = elements;
        _size = _elements.Count();
    }

    private void IncrementRoundRobinIndex() => _roundRobinIndex = (_roundRobinIndex + 1) % _size;

    public string GetNextElement()
    {
        IncrementRoundRobinIndex();
        return _elements.ElementAt(_roundRobinIndex);
    }
}
