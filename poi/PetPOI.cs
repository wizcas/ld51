using System.Threading.Tasks;
using Godot;

public class PetPOI : POI
{
  [Export] public NeedType type;


  protected override async Task PetEnter(Pet pet)
  {
    await pet.Needs.Clear(type, WorkTime);
  }
}