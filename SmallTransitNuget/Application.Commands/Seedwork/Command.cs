namespace Application.Commands.Seedwork;

internal abstract class Command
{
    internal string CommandName => GetType().Name;
}