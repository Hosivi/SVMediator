namespace SVMediator.Pipeline;

public class PipelineTestNotification:INotification
{
    
}

public class PipelineTestCommad : ICommand<PipelineTestNotification, Unit>
{
    public async Task<Unit> ExecuteAsync(PipelineTestNotification notification, object[]? receivers)
    {
        Console.WriteLine("se testeó el pipeline");
        return new Unit();
    }
}