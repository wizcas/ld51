using System;
using System.Threading.Tasks;

public class Pat : POI
{
  protected override async Task PlayerEnter(Player player)
  {
    await base.PlayerEnter(player);
    var pet = Global.Instance.Pet;
    player.Freeze(true);
    pet.Freeze(true);
    pet.Love(true);
    await pet.Needs.Clear(NeedType.Love, WorkTime);
    pet.Love(false);
    player.Freeze(false);
    pet.Freeze(false);
  }
}