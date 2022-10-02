using System;
using System.Threading.Tasks;
using Godot;

public class LitterBox : PetPOI
{
  [Export] public int MaxFullness;
  public int Fullness;

  protected override bool CanPlayerUse => Fullness > 0;
  protected override bool CanPetUse => Fullness < MaxFullness;

  protected override async Task PlayerEnter(Player player)
  {
    await Task.Delay(TimeSpan.FromSeconds(WorkTime));
    Fullness = 0;
  }
}