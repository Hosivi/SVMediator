using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SVMediator;

public static class ServiceExtensions
{
    public static IServiceCollection AddSVMediator(this IServiceCollection service, ServiceLifetime lifetime,
        params Type[] markers)
    {
        var commandsInfo = new Dictionary<Type, Type>();
        foreach (var marker in markers)
        {
            var assembly = marker.Assembly;
            var commands = GetClassesImplementingGenericInterface(assembly, typeof(ICommand<,>));
            var notifications = GetClassesImplementingInterface(assembly, typeof(INotification));
            notifications.ForEach(x =>
            {
                commandsInfo[x] =
                    commands.SingleOrDefault(xx => x == xx.GetInterface("ICommand`2")!.GetGenericArguments()[0]);     
            });
            var serviceDescriptor = commands.Select(x => new ServiceDescriptor(x, x, lifetime));
            service.TryAdd(serviceDescriptor);
        }

        service.AddSingleton<IMediator>(x => new SVMediator(commandsInfo, x.GetRequiredService));
        return service;
    }

    private static List<Type> GetClassesImplementingGenericInterface(Assembly assembly, Type typeToMatch)
    {
        var commands = assembly.ExportedTypes.Where(type =>
        {
            var genericInterfacesTypes = type.GetInterfaces().Where(x => x.IsGenericType).ToList();
            var implementedCommands = genericInterfacesTypes.Any(x => x.GetGenericTypeDefinition() == typeToMatch);
            return !type.IsInterface && !type.IsAbstract && implementedCommands;
        }).ToList();
        return commands;
    }
    private static List<Type> GetClassesImplementingInterface(Assembly assembly, Type typeToMatch)
    {
        var commands = assembly.ExportedTypes.Where(type => type.IsAssignableTo(typeToMatch)).ToList();
         return commands;
    }
}