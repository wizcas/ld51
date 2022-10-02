using System;
using Godot;

public class RoundTime : TextureProgress
{
  #region Nested
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  private Label _second;
  private Label _ms;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    _second = GetNode<Label>("Second");
    _ms = GetNode<Label>("Ms");
    Global.Instance.TenSec.Connect(nameof(GlobalTimer.TimeFlying), this, nameof(OnRoundTimeFlying));
  }

  #endregion

  #region Methods
  private void OnRoundTimeFlying(float timeLeft)
  {
    _second.Text = timeLeft.ToString("0");
    _ms.Text = timeLeft.ToString("0.000").Substring(2);
    Value = 10 - timeLeft;
  }
  #endregion
}