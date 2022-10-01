using System.Threading.Tasks;
using Godot;
public class WorkStation : PlayerPOI
{
  protected override Task Enter(Player player)
  {
    player.CurrentAction = Player.Action.Working;
    return Task.CompletedTask;
  }

  protected override void Leave(Player player)
  {
    player.CurrentAction = Player.Action.Normal;
  }
}