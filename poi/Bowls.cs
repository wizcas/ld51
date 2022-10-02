
using System;
using System.Threading.Tasks;
using Godot;

public class Bowls : PetPOI
{
  [Export] public int MaxFullness;
  public int Fullness;

  private Food _food;

  protected override bool CanPlayerUse => Fullness < MaxFullness;
  protected override bool CanPetUse => Fullness > 0;

  protected override async Task PetEnter(Pet pet)
  {
    await base.PetEnter(pet);
    if (_food != null)
    {
      pet.Needs.Happiness += _food.Happiness;
    }
  }

  protected override async Task PlayerEnter(Player player)
  {
    await base.PlayerEnter(player);
    if (player.Inventory.Food == null) return;

    await Task.Delay(TimeSpan.FromSeconds(WorkTime));

    Fullness = MaxFullness;
    _food = player.Inventory.Food;
  }
}