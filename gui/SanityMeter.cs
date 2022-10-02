public class SanityMeter : ValueMeter
{
  protected override bool HasValue()
  {
    return Global.Instance.Player != null;
  }
  protected override float GetMeterValue()
  {
    return Global.Instance.Player.Sanity.Sanity;
  }
}