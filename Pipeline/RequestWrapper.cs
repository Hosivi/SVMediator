using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace SVMediator.Pipeline;

public interface IHandlerWrapperBase
{
	Task<TResponse> Handle<TResponse>(ICommand<INotification, TResponse> command,INotification notification,IServiceProvider serviceProvider,object[]? receivers,CancellationToken cancellationToken=default);
}
public abstract class HandlerWrapperBase
{
 public abstract Task<object?> Handle(object request, IServiceProvider serviceProvider,object[] receivers,CancellationToken cancellationToken);

}

public abstract class NotificationHandlerWrapperBase<TNotification,TResponse> where TNotification :INotification
{ 
	public abstract Task<TResponse> Handle(TNotification notification, object command, object[] parameters, CancellationToken cancellationToken=default);
}


public class RequestHandlerWrapper<TRequest, TResponse> : HandlerWrapperBase
{
    public async Task<TResponse> Handle(TRequest request, IServiceProvider serviceProvider,object[] receivers, CancellationToken cancellationToken)
    {
        async Task<TResponse> Handle()
        {
            return await serviceProvider.GetRequiredService<IRequestHandler<TRequest,TResponse>>().Handle(request, cancellationToken);
        }

        return await serviceProvider.
            GetServices<IPipeline<TRequest, TResponse>>()
            .Reverse()
            .Aggregate((RequestHandlerDelegate<TResponse>)Handle,
                (next, pipeline) => () => pipeline.Handle(request, next, cancellationToken))();
    }

    public override Task<object?> Handle(object request, IServiceProvider serviceProvider, object[] receivers, CancellationToken cancellationToken)
    {
	    throw new NotImplementedException();
    }
}
public class NotificationHandlerWrapperImpl<TNotification, TResponse> :NotificationHandlerWrapperBase<TNotification,TResponse>  where TNotification:INotification
{
	private readonly IServiceProvider ServiceProvider;

	public NotificationHandlerWrapperImpl(IServiceProvider serviceProvider)
	{
		ServiceProvider = serviceProvider;
	}

	public override async Task<TResponse> Handle(TNotification notification, object command, object[] parameters, CancellationToken cancellationToken = default)
	{
		async Task<TResponse> Handle()
		{
			return await (Task<TResponse>)command.GetType().
				 GetMethod("ExecuteAsync")?.
				 Invoke(command, parameters);
		}

		return await ServiceProvider.
			GetServices<IPipeline<TNotification, TResponse>>()
			.Reverse()
			.Aggregate((RequestHandlerDelegate<TResponse>)Handle,
				(next, pipeline) => () => pipeline.Handle((TNotification)notification, next, cancellationToken))();
	}
}