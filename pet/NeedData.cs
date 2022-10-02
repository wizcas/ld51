
using System;
using Godot;

public class NeedData
{
  public NeedType Type;
  public float Value;
  public float Rate;
  public int Increment;
  public float BoostChance;
  public float BoostMultiplier = .5f;
  public float BoostRandomness = 1f;
  public float HappinessDamage;

  private float _nextTickMs;

  public string Name => Enum.GetName(typeof(NeedType), Type);

  public bool IsUnbearable => Value >= NeedSystem.MAX_NEED_VALUE;

  public NeedData(NeedType type)
  {
    Type = type;
    Value = 0;
    Rate = 1;
    Increment = 1;
    UpdateNextTick();
  }
  public void Process(float delta)
  {
    if (OS.GetTicksMsec() > _nextTickMs)
    {
      Value += Increment;
      if (GD.Randf() < BoostChance)
      {
        Value += Increment * BoostMultiplier * (GD.Randf() + BoostRandomness);
      }
      UpdateNextTick();
    }
  }
  public void Clear()
  {
    Value = 0;
  }
  private void UpdateNextTick()
  {
    _nextTickMs = OS.GetTicksMsec() + Rate * 1000;
  }

}