namespace Application.Queries.Seedwork;

internal abstract class Query
{
    internal string QueryName => GetType().Name;
}