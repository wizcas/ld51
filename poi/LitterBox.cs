using System;
using System.Threading.Tasks;
using Godot;

public class LitterBox : PetPOI
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

  protected override bool CanPlayerUse => Fullness > 0;
  protected override bool CanPetUse => Fullness < MaxFullness;

  private Sprite Sprite => GetNode<Sprite>("Sprite");

  public override void _Ready()
  {
    base._Ready();
  }

  protected override async Task PlayerEnter(Player player)
  {
    await Task.Delay(TimeSpan.FromSeconds(WorkTime));
    Fullness = 0;
  }
  protected override async Task PetEnter(Pet pet)
  {
    await base.PetEnter(pet);
    Fullness++;
    pet.Needs.Happiness += HappinessGain;
  }
}