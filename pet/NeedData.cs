
using System;
using System.Threading.Tasks;
using Godot;

public class NeedData
{
  public NeedType Type;
  public int Value;
  public float Rate;
  public int Increment;

  private float _nextTickMs;

  public string Name => Enum.GetName(typeof(NeedType), Type);

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
      UpdateNextTick();
      GD.Print($"[{Name}] clear! => {Value}");
    }
  }
  public async Task WaitClear(float duration)
  {
    await Task.Delay((int)(duration * 1000));
    Value = 0;
  }
  private void UpdateNextTick()
  {
    _nextTickMs = OS.GetTicksMsec() + Rate * 1000;
  }

}