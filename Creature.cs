using Godot;

public abstract class Creature : KinematicBody2D
{
  [Export] public float Speed = 100;
  #region Private fields
  protected NavigationAgent2D _navAgent;
  protected Vector2 _velocity;
  protected bool _isNavigating;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    base._Ready();
    _navAgent = GetNode<NavigationAgent2D>("NavAgent");
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
  public void Stop()
  {
    _isNavigating = false;
  }
  public void SetDestination(Vector2 gPos)
  {
    _navAgent.SetTargetLocation(gPos);
    _isNavigating = true;
    Global.Instance.Arrow.ShowAt(_navAgent.GetFinalLocation());
    if (!_navAgent.IsTargetReachable())
    {
      GD.PrintErr("target not reachable: ", gPos);
      Stop();
    }
  }
  #endregion

}