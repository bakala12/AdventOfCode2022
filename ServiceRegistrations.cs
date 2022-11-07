using AdventOfCode2022.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode2022
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddAoc(this IServiceCollection serviceCollection)
        {
            return serviceCollection.RegisterInputParsers().RegisterDays();
        }

        private static IServiceCollection RegisterDays(this IServiceCollection serviceCollection)
        {
            var days = typeof(ServiceRegistrations).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType && t.GetInterfaces().Any(i => i == typeof(IDay)));
            foreach (var day in days)
                serviceCollection.AddScoped(day);
            return serviceCollection;
        }

        private static IServiceCollection RegisterInputParsers(this IServiceCollection serviceCollection)
        {
            var inputParsers = typeof(ServiceRegistrations).Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract &&
                t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInputParser<>)));
            foreach (var inputParser in inputParsers)
                foreach (var interfaceType in inputParser.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInputParser<>)))
                    serviceCollection.AddScoped(interfaceType, inputParser);
            return serviceCollection;
        }
    }
}