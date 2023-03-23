using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using SVMediator.Pipeline;

namespace SVMediator;

/// <summary>
/// Este mediador està enlazado a varios componentes no logro desacoplarlo
/// </summary>
public class SVMediator : IMediator
{
    private readonly List<IReceiver> receivers;
    private readonly ConcurrentDictionary<Type, Type> Commands;
    private readonly Dictionary<Type, Type> Requests;
    private readonly Func<Type, object> ServiceResolver;
    private readonly IServiceProvider ServiceProvider;

    public SVMediator(ConcurrentDictionary<Type, Type> commands, Func<Type, object> serviceResolver)
    {
        Commands = commands;
        ServiceResolver = serviceResolver;
    }

    public SVMediator(ConcurrentDictionary<Type, Type> commands, Dictionary<Type, Type> requests, Func<Type, object> serviceResolver)
    {
        Commands = commands;
        Requests = requests;
        ServiceResolver = serviceResolver;
    }

    public SVMediator(ConcurrentDictionary<Type, Type> commands, Dictionary<Type, Type> requests, Func<Type, object> serviceResolver, IServiceProvider serviceProvider)
    {
        Commands = commands;
        Requests = requests;
        ServiceResolver = serviceResolver;
        ServiceProvider = serviceProvider;
    }

    public void Suscribe(IReceiver receiver)
    {
        receivers.Add(receiver);
    }

    public async Task<TResponse> PublishAsync<TResponse>(INotification notification, object? args, object sender, object[]? receivers)
    {
        if (!Commands.ContainsKey(notification.GetType()))
        {
            throw new Exception("No existe el comando");
        }

        Commands.TryGetValue(notification.GetType(), out var commandType);
        var commandInstance = ServiceResolver(commandType);
        object[] parameters = new object[] { notification, receivers, };
        var wrapper = new NotificationHandlerWrapperImpl<INotification, TResponse>(ServiceProvider);
		return await wrapper.Handle(notification, commandInstance, parameters); 

        // var handlerWrapper =
        //     Activator.CreateInstance(
        //         typeof(NotificationHandlerWrapperImpl<,>).MakeGenericType(notification.GetType(), typeof(TResponse)));
        // var methodInfo = handlerWrapper.GetType().GetMethod("Handle");
        // return await Task.FromResult((TResponse)methodInfo.Invoke(handlerWrapper, parameters));
        // return await wrapperInstance.Handle(notification, ServiceProvider, receivers);
        // return await (Task<TResponse>)commandInstance.GetType().
        //     GetMethod("ExecuteAsync").
        //     Invoke(commandInstance, parameters);
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, object? sender, object? receivers = null)
    {
        if (!Requests.ContainsKey(request.GetType()))
        {
            throw new Exception("No existe el comando");
        }

        Requests.TryGetValue(request.GetType(), out var requestType);
        var requestInstance = ServiceResolver(requestType);
        object[] parameters = new object[] { request, receivers };
        return await (Task<TResponse>)requestInstance.GetType().GetMethod("handle").Invoke(requestInstance, parameters);
    }

    private NotificationHandlerWrapperImpl<TNotification, TResponse> GetWrapperInstance<TNotification, TResponse>(TNotification notification) where TNotification : INotification
    {
        Type wrapperType = typeof(NotificationHandlerWrapperImpl<,>).MakeGenericType(notification.GetType(), typeof(TResponse));
        //NotificationHandlerWrapper<INotification, TResponse> notificationHandlerWrapper = new NotificationHandlerWrapper<INotification, TResponse>();
        var wrapperInstance = (NotificationHandlerWrapperImpl<TNotification, TResponse>)Activator.CreateInstance(wrapperType);
        return wrapperInstance;
    }
}