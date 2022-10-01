using System.Threading.Tasks;

public class Bed : PlayerPOI
{
  protected override Task Enter(Player player)
  {
    player.CurrentAction = Player.Action.Sleeping;
    return Task.CompletedTask;
  }

  protected override void Leave(Player player)
  {
    player.CurrentAction = Player.Action.Normal;
  }
}