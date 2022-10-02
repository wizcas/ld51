using Godot;
public class SanityMeter : ValueMeter
{
  private Color _normalColor;

  public override void _Ready()
  {
    base._Ready();
    _normalColor = Colors.White;
  }
  protected override bool HasValue()
  {
    return Global.Instance.Player != null;
  }
  protected override float GetMeterValue()
  {
    return (int)MaxValue - Global.Instance.Player.Sanity.Sanity;
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    TintProgress = Value <= MaxValue * .3f ? Colors.Red : _normalColor;
  }
}