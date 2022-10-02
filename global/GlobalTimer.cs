using System;
using Godot;

public class GlobalTimer : Timer
{
  #region Nested
  #endregion

  #region Signals
  [Signal] public delegate void TimeFlying(float timeLeft);
  #endregion

  #region Fields & Properties
  #endregion

  #region Hooks
  public override void _Ready()
  {
    Connect("timeout", this, nameof(OnSelfTimeOut));
  }
  public override void _Process(float delta)
  {
    base._Process(delta);
    EmitSignal(nameof(TimeFlying), TimeLeft);
  }
  #endregion

  #region Methods
  public void OnSelfTimeOut()
  {
    Start();
  }
  #endregion
}