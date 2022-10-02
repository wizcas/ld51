using System;
using System.Threading.Tasks;

public class Fridge : POI
{
  protected override async Task PlayerEnter(Player player)
  {
    await base.PlayerEnter(player);
    await Task.Delay(TimeSpan.FromSeconds(WorkTime));
    player.Inventory.TakeFood(new Food(FoodType.Can, 10));
  }
}