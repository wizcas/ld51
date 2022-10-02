using System.Threading.Tasks;
using Godot;

public class PetPOI : POI
{
  [Export] public NeedType Type;
  [Export] public int HappinessGain = 10;

  protected override async Task PetEnter(Pet pet)
  {
    await pet.Needs.Clear(Type, WorkTime);
  }
}