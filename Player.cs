using System;
using Godot;

public class Player : KinematicBody2D
{
  #region Nested
  [Export] public float Speed = 200;
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  private NavigationAgent2D _navAgent;
  private Vector2 _velocity;
  private bool _isNavigating;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    _navAgent = GetNode<NavigationAgent2D>("NavAgent");
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventMouseButton mouseEvent)
    {
      if (mouseEvent.ButtonIndex == (int)ButtonList.Left)
      {
        var _target = GetGlobalMousePosition();
        _navAgent.SetTargetLocation(_target);
        _isNavigating = true;
        if (!_navAgent.IsTargetReachable())
        {
          GD.PrintErr("target not reachable: ", _target);
          _isNavigating = false;
        }
      }
    }

    if (_isNavigating)
    {
      ArrowMarker.Instance.ShowAt(_navAgent.GetFinalLocation());
    }
    else
    {
      ArrowMarker.Instance.Hide();
    }
  }

  public override void _PhysicsProcess(float delta)
  {
    base._PhysicsProcess(delta);
    if (!_isNavigating)
    {
      _velocity = Vector2.Zero;
      return;
    }

    if (!_navAgent.IsTargetReachable())
    {
      _isNavigating = false;
      return;
    }
    if (_navAgent.IsTargetReached())
    {
      _isNavigating = false;
    }
    else
    {
      var next = _navAgent.GetNextLocation();
      _velocity = (next - Position).Normalized() * Speed;
    }
    _velocity = MoveAndSlide(_velocity);
  }

  #endregion

  #region Methods
  #endregion
}