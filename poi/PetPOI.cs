using System.Threading.Tasks;
using Godot;

public class PetPOI : POI
{
  [Export] public NeedType type;


  protected override sealed async Task Enter(Creature worker)
  {
    await base.Enter(worker);
    if (worker is Pet pet)
    {
      await Enter(pet);
    }
  }
  protected sealed override void Leave(Creature worker)
  {
    base.Leave(worker);
    if (worker is Pet pet)
    {
      Leave(pet);
    }
  }
  protected virtual async Task Enter(Pet pet)
  {
    await pet.Needs.Clear(type, WorkTime);
  }
  protected virtual void Leave(Pet pet) { }
}