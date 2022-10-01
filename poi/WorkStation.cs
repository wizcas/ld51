using System.Threading.Tasks;
using Godot;
public class WorkStation : POI
{
  protected override Task PlayerEnter(Player player)
  {
    player.CurrentAction = Player.Action.Working;
    return Task.CompletedTask;
  }

  protected override void PlayerLeave(Player player)
  {
    player.CurrentAction = Player.Action.Normal;
  }
}