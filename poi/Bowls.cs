
using System;
using System.Threading.Tasks;
using Godot;

public class Bowls : PetPOI
{
  [Export] public int MaxFullness = 3;
  [Export] public Texture[] FullnessTextures;
  private int _fullness;
  public int Fullness
  {
    get { return _fullness; }
    set
    {
      _fullness = (int)Mathf.Clamp(value, 0, MaxFullness);
      Sprite.Texture = FullnessTextures[_fullness >= FullnessTextures.Length ? FullnessTextures.Length - 1 : _fullness];
    }
  }

  private Food _food;

  protected override bool CanPlayerUse => Fullness < MaxFullness;
  protected override bool CanPetUse => Fullness > 0;
  private Sprite Sprite => GetNode<Sprite>("Sprite");

  protected override async Task PetEnter(Pet pet)
  {
    await base.PetEnter(pet);
    if (_food != null)
    {
      pet.Needs.Happiness += _food.Happiness + HappinessGain;
      Fullness--;
    }
  }

  protected override async Task PlayerEnter(Player player)
  {
    await base.PlayerEnter(player);
    if (player.Inventory.Food == null) return;

    await Task.Delay(TimeSpan.FromSeconds(WorkTime));

    Fullness = MaxFullness;
    _food = player.Inventory.Food;
    player.Inventory.Food = null;
  }
}