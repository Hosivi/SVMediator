namespace SVMediator;

/// <summary>
/// Interfaces Para las Queries
/// </summary>
/// <typeparam name="IRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IRequestHandler<TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface ICommand<TNotification, TResponse> where TNotification:INotification 
{
    Task<TResponse> ExecuteAsync(TNotification notification, object[]? receivers);
}

public interface IRequest<TResponse>
{
}
