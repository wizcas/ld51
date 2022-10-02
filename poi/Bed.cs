using System.Threading.Tasks;

public class Bed : POI
{
  protected override Task PlayerEnter(Player player)
  {
    ToggleWarn(false);
    player.CurrentAction = Player.Action.Sleeping;
    return Task.CompletedTask;
  }

  protected override void PlayerLeave(Player player)
  {
    ToggleWarn(true);
    player.CurrentAction = Player.Action.Normal;
  }
}