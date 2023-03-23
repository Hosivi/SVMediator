namespace SVMediator;

public interface IMediator
{
    void Suscribe(IReceiver receiver); 
    //Task<TResponse?> PublishAsync<TResponse>(INotification notification, object? args, object sender);
	/// <summary>
	/// PublishAsyncWithArgsArray
	/// Eliminado por cruce en la creación de comandos de sin argumentos
	/// </summary>
	/// <typeparam name="TResponse"></typeparam>
	/// <param name="notification"></param>
	/// <param name="args"></param>
	/// <param name="sender"></param>
	/// <param name="receivers"></param>
	/// <returns></returns>
	//Task<TResponse?> PublishAsync<TResponse>(INotification notification, object[]? args, object sender, List<object> receivers);
	//Task<TResponse> PublishAsync<TResponse>(IRequest<>)
	Task<TResponse> PublishAsync<TResponse>(INotification notification, object? args, object sender, object[]? receivers);

	Task<TResponse> Send<TResponse>(IRequest<TResponse> request, object? sender, object? receivers = null);
}

public interface IReceiver
{
    Task ReponseAsync(object? args ); 
}

public interface INotification
{

}

public interface IValidationService
{

}

public interface IPaginatorNotification<T>
{
	public int Page { get; set; }
	public int PageSize { get; set; }
	public T? Searcher { get; set; }
	public bool IsPaginator { get; set; }
}

