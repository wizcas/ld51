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

  private async void OnPetShouting(Pet pet)
  {
    _isFrozen = true;
    await Task.Delay(TimeSpan.FromSeconds(FreezeTime));
    _isFrozen = false;
  }

  #endregion
}