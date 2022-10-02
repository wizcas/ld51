
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
      ToggleWarn(_fullness == 0);
    }
  }

  private Food _food;

  protected override bool CanPlayerUse(Player player) => Fullness < MaxFullness && player.Inventory.Food != null;
  protected override bool CanPetUse(Pet pet) => Fullness > 0;
  private Sprite Sprite => GetNode<Sprite>("Sprite");

  public override void _Ready()
  {
    base._Ready();
    Fullness = 0;
  }

  protected override async Task PetEnter(Pet pet)
  {
    if (_food != null)
    {
      pet.PlaySound(PetSound.Eat);
      await base.PetEnter(pet);
      pet.Needs.Happiness += _food.Happiness + HappinessGain;
      if (--Fullness <= 0) _food = null;
    }
  }

  protected override async Task PlayerEnter(Player player)
  {
    await base.PlayerEnter(player);
    if (player.Inventory.Food == null) return;

    await Task.Delay(TimeSpan.FromSeconds(WorkTime));

    Fullness = MaxFullness;
    _food = player.Inventory.Food;
    player.Inventory.ClearFood();
  }
}