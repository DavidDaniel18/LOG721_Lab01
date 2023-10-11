namespace SmallTransit.Application.Requests;

internal abstract class Command
{
    internal string CommandName => GetType().Name;
}