using Godot;

public class NeedMeter : ValueMeter
{
  [Export] public NeedType Type;
  private NeedData _data;

  protected override bool HasValue()
  {
    _data = Global.Instance.Pet?.Needs?.GetNeed(Type);
    return _data != null;
  }
  protected override float GetMeterValue()
  {
    return _data.Value;
  }
}