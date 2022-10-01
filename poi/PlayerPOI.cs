using System.Threading.Tasks;

public class PlayerPOI : POI
{
  protected override sealed async Task Enter(Creature worker)
  {
    await base.Enter(worker);
    if (worker is Player player)
    {
      await Enter(player);
    }
  }
  protected sealed override void Leave(Creature worker)
  {
    base.Leave(worker);
    if (worker is Player player)
    {
      Leave(player);
    }
  }

  protected virtual async Task Enter(Player player)
  {
    await Task.CompletedTask;
  }

  protected virtual void Leave(Player player) { }
}