using System.Threading.Tasks;
using Godot;

public class PetNeedPOI : POI
{
  [Export] public NeedType type;

  protected override async Task Work(Creature worker)
  {
    if (worker is Pet pet)
    {
      await pet.Needs.Clear(type, WorkTime);
    }
  }
}