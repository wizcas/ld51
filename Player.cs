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
  private POI _poi;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    _navAgent = GetNode<NavigationAgent2D>("NavAgent");
    Global.Instance.Player = this;
  }

  public override void _UnhandledInput(InputEvent e)
  {
    if (e is InputEventMouseButton mouseEvent)
    {
      if (mouseEvent.IsPressed() && mouseEvent.ButtonIndex == (int)ButtonList.Left)
      {
        var _target = GetGlobalMousePosition();
        _navAgent.SetTargetLocation(_target);
        _isNavigating = true;
        Global.Instance.Arrow.ShowAt(_navAgent.GetFinalLocation());
        if (!_navAgent.IsTargetReachable())
        {
          GD.PrintErr("target not reachable: ", _target);
          Stop();
        }
      }
    }

  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    if (!_isNavigating)
    {
      Global.Instance.Arrow.Hide();
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
      Stop();
    }
    else if (_navAgent.IsTargetReached())
    {
      Stop();
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

  public void SetTargetPOI(POI poi)
  {
    _navAgent.SetTargetLocation(poi.GlobalPosition);
    _poi = poi;
    _isNavigating = true;
  }

  public void Stop()
  {
    _isNavigating = false;
  }

  public void Interact(POI poi)
  {
    Stop();
    GD.Print("interacting POI: ", poi.Name);
  }
  #endregion
}