using System;
using System.Threading.Tasks;
using Godot;
public class WorkStation : POI
{
  [Export] NodePath ComputerNode;

  private AnimatedSprite _computer;


  public override void _Ready()
  {
    base._Ready();
    _computer = GetNode<AnimatedSprite>(ComputerNode);
    ToggleWarn(true);
  }
  protected override async Task PlayerEnter(Player player)
  {
    ToggleWarn(false);
    player.CurrentAction = Player.Action.Working;
    _computer.Playing = true;
    await Task.Delay(TimeSpan.FromSeconds(WorkTime + .05));
    player.ToggleBusyCloud(true);
  }

  protected override void PlayerLeave(Player player)
  {
    ToggleWarn(true);
    _computer.Playing = false;
    player.CurrentAction = Player.Action.Normal;
    player.ToggleBusyCloud(false);
  }
}