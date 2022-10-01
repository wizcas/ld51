using System;
using Godot;

public class ArrowMarker : Node2D
{
  #region Nested
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  private AnimationPlayer _anim;

  #endregion

  #region Hooks
  public override void _Ready()
  {
    Hide();
    _anim = GetNode<AnimationPlayer>("Anim");
  }
  #endregion

  #region Methods
  public void ShowAt(Vector2 gPos)
  {
    GlobalPosition = gPos;
    Show();
    _anim.Play("floating");
  }
  #endregion
}