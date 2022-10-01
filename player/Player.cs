using System.Threading.Tasks;
using Godot;

public class Player : Creature
{
  #region Nested
  public enum Action
  {
    Normal,
    Working,
    Sleeping,
  }
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  public Action CurrentAction;
  private POI _poi;
  #endregion

  #region Hooks

  public override void _EnterTree()
  {
    base._EnterTree();
    Global.Instance.Player = this;
  }

  public override void _UnhandledInput(InputEvent e)
  {
    if (e is InputEventMouseButton mouseEvent)
    {
      if (mouseEvent.IsPressed() && mouseEvent.ButtonIndex == (int)ButtonList.Left)
      {
        CancelForceMove();
        SetNavTarget(GetGlobalMousePosition());
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

  #endregion

  #region Methods

  public void SetTargetPOI(POI poi)
  {
    _poi = poi;
    SetNavTarget(poi.GetDestination(this));
  }

  public async override Task Interact(POI poi)
  {
    Stop();
    ForceMoveTo(poi.GetDestination(this));
    await Task.CompletedTask;
  }

  #endregion
}