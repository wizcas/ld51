using System;
using Godot;

public class HUD : Node
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
    Global.Instance.TenSec.Connect(nameof(GlobalTimer.TimeFlying), this, nameof(OnTenSecTimeOut));
  }

  #endregion

  #region Methods
  public void OnTenSecTimeOut(float timeLeft)
  {
  }
  #endregion
}