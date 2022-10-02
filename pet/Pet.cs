using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public class Pet : Creature
{
  #region Fields and Properties
  [Signal] public delegate void Shouting(Pet pet);
  [Export] NodePath WanderingAreaNode;

  public NeedSystem Needs { get; private set; }
  private WanderingArea _wanderingArea;
  private float _nextActionTime;
  private CPUParticles2D _loveFX;
  #endregion

  #region Hooks
  public override void _EnterTree()
  {
    base._EnterTree();
    Global.Instance.Pet = this;
  }
  public override void _Ready()
  {
    base._Ready();
    Needs = GetNode<NeedSystem>("NeedSystem");
    _loveFX = GetNode<CPUParticles2D>("LoveFX");
    if (!WanderingAreaNode.IsEmpty())
    {
      _wanderingArea = GetNode<WanderingArea>(WanderingAreaNode);
    }
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    if (_wanderingArea != null && !_isNavigating && !_isFrozen && !_isBusy && OS.GetTicksMsec() > _nextActionTime)
    {
      var location = _wanderingArea.GetRandomLocation();
      if (location.HasValue)
      {
        GD.Print("next wander location @ ", location.Value);
        SetNavTarget(location.Value);
        location = null;
      }
    }
  }
  #endregion

  #region Methods
  public void FulfillNeed(NeedData need)
  {
    var nodes = GetTree().GetNodesInGroup("pet-needs");
    var candidates = new List<PetPOI>();
    foreach (Node node in nodes)
    {
      GD.Print($"checking poi node: {node.Name}, isPetPOI: {node is PetPOI}, type: {(node as PetPOI)?.Type}, want: {need.Type}");
      if (node is PetPOI poi && poi.Type == need.Type)
      {
        candidates.Add(poi);
        GD.Print($"pet poi candicate: {poi.Name} -> {System.Enum.GetName(typeof(NeedType), poi.Type)}");
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
      GD.Print($"pet is going to POI: {poi.Name} @ {poi.GetDestination(this)}");
      SetNavTarget(poi.GetDestination(this));
      Think();
    }
  }
  public async override Task Interact(POI poi)
  {
    _isBusy = true;
    Stop();
    ForceMoveTo(poi.GetDestination(this));
    await Task.Delay(TimeSpan.FromSeconds(poi.WorkTime));
    CancelForceMove();
    _isBusy = false;
  }
  public async Task Shout()
  {
    _isBusy = true;
    Stop();
    EmitSignal(nameof(Shouting), this);
    await Task.Delay(TimeSpan.FromSeconds(.5f));
    Think();
    _isBusy = false;
  }
  public override void Stop()
  {
    base.Stop();
    Think();
  }

  public void Think()
  {
    if (_isNavigating) return;
    _nextActionTime = OS.GetTicksMsec() + 1000;
  }

  public void Love(bool on)
  {
    _loveFX.Emitting = on;
  }
  #endregion
}