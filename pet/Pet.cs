using System.Collections.Generic;
using System.Linq;
using Godot;

public class Pet : Creature
{
  [Export] NodePath WanderingAreaNode;

  private WanderingArea _wanderingArea;

  public override void _Ready()
  {
    base._Ready();
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
        GD.Print("wondering to: ", location.Value);
        SetDestination(location.Value);
      }
    }
  }

  public void Go(NeedData need)
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
      GD.Print("[pet] is going to: ", poi.Name);
      SetDestination(poi.GetDestination(this));
    }
  }

  public async void Interact(POI poi, Vector2? attachPoint)
  {
    _isBusy = true;
    Stop();
    if (attachPoint.HasValue)
    {
      GlobalPosition = attachPoint.Value;
    }
    await ToSignal(GetTree().CreateTimer(2), "timeout");
    _isBusy = false;
  }
}