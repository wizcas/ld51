using System;
using Godot;

public class StartScreen : ColorRect
{
  #region Nested
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  #endregion

  #region Hooks
  public override void _Ready()
  {
    GetNode<Button>("StartButton").Connect("pressed", this, nameof(OnStartButtonPressed));
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    base._UnhandledInput(@event);
    if (!Visible) return;
    if (@event.IsActionPressed("ui_accept"))
    {
      OnStartButtonPressed();
    }
  }
  #endregion

  #region Methods
  private void OnStartButtonPressed()
  {
    Global.Instance.StartGame();
  }
  #endregion
}