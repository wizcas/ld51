using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public class Pet : Creature
{
  #region Fields and Properties
  [Export] NodePath WanderingAreaNode;

  public NeedSystem Needs { get; private set; }
  private WanderingArea _wanderingArea;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    base._Ready();
    Needs = GetNode<NeedSystem>("NeedSystem");
    if (!WanderingAreaNode.IsEmpty())
    {
      _wanderingArea = GetNode<WanderingArea>(WanderingAreaNode);
    }
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    if (_wanderingArea != null && !_isNavigating && !_isBusy)
    {
      var location = _wanderingArea.GetRandomLocation();
      if (location.HasValue)
      {
        SetNavTarget(location.Value);
      }
    }
  }
  #endregion

  #region Methods
  public void GoForNeed(NeedData need)
  {
    var nodes = GetTree().GetNodesInGroup("pet-needs");
    var candidates = new List<PetNeedPOI>();
    foreach (var node in nodes)
    {
      if (node is PetNeedPOI poi && poi.type == need.Type)
      {
        candidates.Add(poi);
      }
    }
    if (candidates.Count == 0)
    {
      // TODO: show warning on UI to let the player know
      GD.PrintErr("cannot meet pet's need for no poi found: ", need.Name);
    }
    else
    {
      var rnd = (int)(GD.Randf() * candidates.Count);
      var poi = candidates[rnd];
      SetNavTarget(poi.GetDestination(this));
    }
  }

  public async override Task Interact(POI poi)
  {
    _isBusy = true;
    Stop();
    ForceMoveTo(poi.GetDestination(this));
    await ToSignal(GetTree().CreateTimer(2), "timeout");
    CancelForceMove();
    _isBusy = false;
  }
  public async void Shout()
  {
    _isBusy = true;
    Stop();
    var duration = .2f;
    Global.Instance.Camera.StartShaking(duration);
    Global.Instance.Meow.Pop(this);
    await Task.Delay(TimeSpan.FromMilliseconds(duration * 1000));
    _isBusy = false;
  }
  #endregion
}