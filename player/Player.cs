using System;
using System.Collections.Generic;
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

  private Action _currentAction;
  public Action CurrentAction
  {
    get { return _currentAction; }
    set
    {
      _currentAction = value;
      _zzz.Emitting = value == Action.Sleeping;
    }
  }
  private POI _poi;
  private List<POI> _poiQueue = new List<POI>();
  private Node2D _busyCloud;
  private CPUParticles2D _zzz;
  public SanitySystem Sanity { get; private set; }
  public Inventory Inventory { get; private set; }
  protected override bool IsBusy
  {
    get => base.IsBusy;
    set
    {
      base.IsBusy = value;
      ToggleBusyCloud(value);
    }
  }
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
    _busyCloud = GetNode<Node2D>("BusyCloud");
    _zzz = GetNode<CPUParticles2D>("Zzz");
    IsBusy = false;
    _zzz.Emitting = false;
    Global.Instance.Pet.Connect(nameof(Pet.Shouting), this, nameof(OnPetShouting));
  }

  public override void _UnhandledInput(InputEvent e)
  {
    if (e is InputEventMouseButton mouseEvent)
    {
      if (mouseEvent.IsPressed() && mouseEvent.ButtonIndex == (int)ButtonList.Left)
      {
        ClearAllPOIs();
        CancelForceMove();
        SetNavTarget(GetGlobalMousePosition());
      }
    }

  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    GotoNextPOI();
    if (!_isNavigating)
    {
      Global.Instance.Arrow.Hide();
    }
  }

  #endregion

  #region Methods

  public void AddPOI(POI poi)
  {
    if (poi == _poi) return;
    var index = _poiQueue.IndexOf(poi);
    if (index < 0)
    {
      _poiQueue.Add(poi);
    }
  }
  public void ForgetPOI(POI poi)
  {
    if (poi == _poi)
    {
      _poi = null;
    }
    if (_poiQueue.Contains(poi))
    {
      _poiQueue.Remove(poi);
    }
  }
  public void ClearAllPOIs()
  {
    _poi = null;
    _poiQueue.Clear();
  }

  private void GotoNextPOI()
  {
    if (_poi != null) return; // already working on a POI. Go to next once it's done.
    if (_poiQueue.Count == 0) return;
    _poi = _poiQueue[0];
    _poiQueue.RemoveAt(0);
    SetNavTarget(_poi.GetDestination(this));
  }

  public bool IsCurrentPOI(POI poi)
  {
    return poi == _poi;
  }

  public async override Task Interact(POI poi)
  {
    IsBusy = true;
    Stop();
    ForceMoveTo(poi.GetDestination(this));
    await Task.Delay(TimeSpan.FromSeconds(poi.WorkTime));
    if (poi == _poi)
    {
      // clear the active POI if no following ones.
      ForgetPOI(poi);
    }
    IsBusy = false;
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
  public void ToggleBusyCloud(bool show)
  {
    _busyCloud.Visible = show;
  }

  #endregion
}