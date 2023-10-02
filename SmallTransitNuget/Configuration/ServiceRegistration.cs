using Configuration.Options;

namespace Configuration;

public static class ServiceRegistration
{
    public static IServiceCollection AddSmallTransit(this IServiceCollection collection, Action<SmallTransitConfigurator>? configure = null)
    {
        var configuration = new SmallTransitConfigurator();

        configure?.Invoke(configuration);



        return collection;
    }
}