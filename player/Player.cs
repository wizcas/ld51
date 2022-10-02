using System;
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
  [Export] public float FreezeTime = .5f;

  public Action CurrentAction;
  private POI _poi;
  public SanitySystem Sanity { get; private set; }
  public Inventory Inventory { get; private set; }
  #endregion

  #region Hooks

  public override void _EnterTree()
  {
    base._EnterTree();
    Global.Instance.Player = this;
  }

  public override void _Ready()
  {
    base._Ready();
    Sanity = GetNode<SanitySystem>("SanitySystem");
    Inventory = GetNode<Inventory>("Inventory");
    Global.Instance.Pet.Connect(nameof(Pet.Shouting), this, nameof(OnPetShouting));
  }

  public override void _UnhandledInput(InputEvent e)
  {
    if (e is InputEventMouseButton mouseEvent)
    {
      if (mouseEvent.IsPressed() && mouseEvent.ButtonIndex == (int)ButtonList.Left)
      {
        _poi = null;
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
    if (poi != null) SetNavTarget(poi.GetDestination(this));
  }

  public async override Task Interact(POI poi)
  {
    Stop();
    ForceMoveTo(poi.GetDestination(this));
    await Task.Delay(TimeSpan.FromSeconds(poi.WorkTime));
    _poi = null;
    GD.PrintErr("poi reset");
  }

  public bool IsCurrentPOI(POI poi)
  {
    return poi == _poi;
  }

  private async void OnPetShouting(Pet pet)
  {
    if (CurrentAction == Action.Sleeping)
    {
      ForceMoveTo(GlobalPosition + Vector2.Right * 16);
    }
    else
    {
      Freeze(true);
      await Task.Delay(TimeSpan.FromSeconds(FreezeTime));
      Freeze(false);
    }
  }

  #endregion
}