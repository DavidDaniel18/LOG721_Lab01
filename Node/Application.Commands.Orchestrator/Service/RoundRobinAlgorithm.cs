﻿namespace Application.Commands.Orchestrator.Service;

public class RoundRobinAlgorithm : IRoundRobinAlgorithm
{
    private readonly List<string> _elements;

    private readonly int _size;

    private int _roundRobinIndex = 0;

    internal RoundRobinAlgorithm(List<string> elements)
    {
        _elements = elements;
        _size = _elements.Count();
    }

    private void IncrementRoundRobinIndex() => _roundRobinIndex = (_roundRobinIndex + 1) % _size;

    public string GetNextElement()
    {
        string topic = _elements.ElementAt(_roundRobinIndex);
        IncrementRoundRobinIndex();
        return topic;
    }
}
