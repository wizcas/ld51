using Godot;

public class EndScreen : Control
{
  public override void _UnhandledInput(InputEvent @event)
  {
    base._UnhandledInput(@event);
    if (!Visible) return;
    if (@event.IsActionPressed("ui_accept"))
    {
      Global.Instance.Restart();
    }
  }
}