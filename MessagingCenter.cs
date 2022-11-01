namespace SVMediator;

public class MessagingCenter
{
	public delegate void StateNotify(object sender, StateArgs? args);
}

public class StateArgs
{
	public string? State { get; set; }
	public string? Message { get; set; }
	public Action? ActionBeforeEvent { get; set; }
	public Action? ActionAfterEvent { get; set; }
	public Action<object>? ActionWithArgAfterEvent { get; set; }
	public Action? ConfirmAction { get; set; }
	public Action? CancelAction { get; set; }
	public object? Sender { get; set; }
	public bool IfQueryMessage { get; set; }=false;

}

public class SVComponentEvents : MessagingCenter
{
	public event  StateNotify OnNotify;

	public void Notify(object sender, StateArgs? e)
	{
		if (OnNotify is not null)
		{
			OnNotify(sender, e);
		}
	}
}