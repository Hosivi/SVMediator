namespace SVMediator;

public interface ICommand<TNotification, TResponse> where TNotification :INotification
{
    Task<TResponse> ExecuteAsync(TNotification notification, object[]? receivers);
}

/// <summary>
/// Interfaces en desuso
/// </summary>
/// <typeparam name="IRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IRequestHandler<IRequest, TResponse>
{
	Task<TResponse> Handle(TResponse request, CancellationToken cancellationToken);
}

public interface IRequest
{

}
