using System.Threading.Tasks;
using Godot;

public abstract class Creature : KinematicBody2D
{
  [Export] public float Speed = 100;
  #region Private fields
  protected NavigationAgent2D _navAgent;
  protected Vector2 _velocity;
  protected bool _isNavigating;
  private bool _isBusy;
  protected virtual bool IsBusy
  {
    get { return _isBusy; }
    set { _isBusy = value; }
  }
  protected bool _isFrozen;

  private Vector2 _forceDestination;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    base._Ready();
    _navAgent = GetNode<NavigationAgent2D>("NavAgent");
    Global.Instance.Connect(nameof(Global.GameReset), this, nameof(OnGameReset));
  }
  public virtual void OnGameReset()
  {
    IsBusy = false;
    _isFrozen = false;
    _isNavigating = false;
    _velocity = Vector2.Zero;
    _forceDestination = Vector2.Zero;
  }
  public override void _PhysicsProcess(float delta)
  {
    base._PhysicsProcess(delta);
    if (_isFrozen)
    {
      MoveAndSlide(Vector2.Zero);
      return;
    }

    if (_isNavigating && !IsBusy)
    {
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
        _velocity = (next - GlobalPosition).Normalized() * Speed;
      }
    }
    else if (_forceDestination != Vector2.Zero)
    {
      var offset = _forceDestination - GlobalPosition;
      var dist = offset.Length();
      _velocity = offset.Normalized() * Speed * Mathf.Min(dist, 16) / 8;
    }
    else
    {
      _velocity = Vector2.Zero;
    }

    if (_velocity != Vector2.Zero) _velocity = MoveAndSlide(_velocity);
  }
  #endregion

  #region Methods
  public virtual void Stop()
  {
    _isNavigating = false;
    _velocity = Vector2.Zero;
  }
  public void SetNavTarget(Vector2 gPos)
  {
    _navAgent.SetTargetLocation(gPos);
    _isNavigating = true;
    if (this is Player)
    {
      Global.Instance.Arrow.ShowAt(_navAgent.GetFinalLocation());
    }
    if (!_navAgent.IsTargetReachable())
    {
      GD.PrintErr("target not reachable: ", gPos);
      Stop();
    }
  }
  public void ForceMoveTo(Vector2 gPos)
  {
    _forceDestination = gPos;
  }
  public void CancelForceMove()
  {
    _forceDestination = Vector2.Zero;
  }

  public void Freeze(bool on)
  {
    _isFrozen = on;
  }

  public abstract Task Interact(POI poi);

  #endregion

}