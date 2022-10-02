using Godot;

public abstract class ValueMeter : TextureProgress
{
  public override void _Process(float delta)
  {
    base._Process(delta);
    if (HasValue()) Value = MaxValue - GetMeterValue();
  }

  protected abstract bool HasValue();
  protected abstract float GetMeterValue();
}