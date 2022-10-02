using System;
using System.Threading.Tasks;

public class Pat : POI
{
  public override void _Ready()
  {
    base._Ready();
    Connect("mouse_entered", this, nameof(OnMouseEntered));
    ToggleWarn(false);
  }

  private async void OnMouseEntered()
  {
    if (IsToggleShown) return;
    var shown = ToggleWarn(true);
    if (!shown) return;
    await Task.Delay(TimeSpan.FromSeconds(.5f));
    ToggleWarn(false);
  }

  protected override async Task PlayerEnter(Player player)
  {
    await base.PlayerEnter(player);
    var pet = Global.Instance.Pet;
    player.Freeze(true);
    pet.Freeze(true);
    pet.Love(true);
    pet.PlaySound(PetSound.Purr, WorkTime);
    await pet.Needs.Clear(NeedType.Love, WorkTime);
    pet.Love(false);
    player.Freeze(false);
    pet.Freeze(false);
  }
}