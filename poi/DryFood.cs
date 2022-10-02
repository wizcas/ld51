
using System;
using System.Threading.Tasks;

public class DryFood : POI
{
  protected override async Task PlayerEnter(Player player)
  {
    await base.PlayerEnter(player);
    await Task.Delay(TimeSpan.FromSeconds(WorkTime));
    player.Inventory.TakeFood(new Food(FoodType.Dry, 5));
  }
}