using System;
using Godot;

public class HUD : Node
{
  #region Nested
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  private Label _nextRoundTimeLabel;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    Global.Instance.TenSec.Connect(nameof(GlobalTimer.TimeFlying), this, nameof(OnTenSecTimeOut));
    _nextRoundTimeLabel = GetNode<Label>("NextRoundTime/Label");
  }

  #endregion

  #region Methods
  public void OnTenSecTimeOut(float timeLeft)
  {
    _nextRoundTimeLabel.Text = timeLeft.ToString("0.000");
  }
  #endregion
}