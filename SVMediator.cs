namespace SVMediator;
 /// <summary>
 /// Este mediador està enlazado a varios componentes no logro desacoplarlo
 /// </summary>
public class SVMediator:IMediator
 { 
	 private readonly List<IReceiver> receivers;
    private readonly Dictionary<Type, Type > Commands;
    private readonly Func<Type, object> ServiceResolver;

    public SVMediator(Dictionary<Type, Type> commands, Func<Type, object> serviceResolver)
    {
        Commands = commands;
        ServiceResolver = serviceResolver;
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
        object[] parameters= new object[] {notification,receivers};
        return await (Task<TResponse>)commandInstance.GetType().GetMethod("ExecuteAsync").Invoke(commandInstance, parameters);
    }
    //public async Task<TResponse?> PublishAsync<TResponse>(INotification notification, object[]? args, object sender, List<object> receivers)
    //{
        
    //    Commands.TryGetValue(notification.GetType(), out var commandType);
    //    if (commandType is not null)
    //    { 
	   //     ICommand<INotification, TResponse>? command =(ICommand<INotification, TResponse>) Activator.CreateInstance(commandType, args)!;
    //        TResponse res = await command.ExecuteAsync(notification);
    //        return res;
    //    }

    //    return default;

    //}
}