namespace SVMediator.Pipeline;


public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
public interface IPipeline<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);

}
public interface IRequestPreProcess<in TRequest>
{
    Task Process(TRequest request, CancellationToken cancellationToken);  
}
public interface IRequestPostProcess<in TRequest, in TResponse>
{
    Task Process(TRequest request, TResponse response, CancellationToken cancellationToken);
}  
