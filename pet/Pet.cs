using System.Collections.Generic;
using System.Linq;
using Godot;

public class Pet : Creature
{
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
      var rnd = (int)GD.Randf() * candidates.Count;
      var poi = candidates[rnd];
      GD.Print("[pet] is going to: ", poi.Name);
      SetDestination(poi.GlobalPosition);
    }
  }
}