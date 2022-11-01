namespace SVMediator;

public abstract class AbstractCommand<TNotification, TResponse> : ICommand<TNotification, TResponse> where TNotification : INotification
{
    private readonly TNotification notification;
    private readonly IValidationService validationService;

    protected AbstractCommand(TNotification notification, IValidationService validationService)
    {
        this.notification = notification;
        this.validationService = validationService;
    }

    public abstract  Task<TResponse> ExecuteAsync(TNotification notification, object[]? receivers);
	
}