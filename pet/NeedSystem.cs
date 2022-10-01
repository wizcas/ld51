using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class NeedSystem : Node
{
  const int MIN_NEED_VALUE = 30;
  const int MAX_NEED_VALUE = 100;
  const float ACTION_INTERVAL_MS = 10 * 1000;
  public float Happiness;

  private float _nextActionTime = 0;
  private List<NeedData> _needs = new List<NeedData>{
    new NeedData(NeedType.Hunger){Rate=5},
    new NeedData(NeedType.Love){Rate=3},
    new NeedData(NeedType.Cleanness){Rate=1},
    new NeedData(NeedType.Defecation){Rate=2,Increment=10},
  };
  private Pet _pet;

  public override void _Ready()
  {
    base._Ready();
    _nextActionTime = OS.GetTicksMsec() + ACTION_INTERVAL_MS;
    _pet = GetParent<Pet>();
    Global.Instance.TenSec.Connect("timeout", this, nameof(OnTenSecTimeOut));
  }

  public override void _Process(float delta)
  {
    base._Process(delta);

    foreach (var need in _needs)
    {
      need.Process(delta);
      if (need.Value >= MAX_NEED_VALUE)
      {
        Mad(need.Type);
      }
    }
    if (OS.GetTicksMsec() >= _nextActionTime)
    {
      GD.Print("10 sec!");
      _nextActionTime = OS.GetTicksMsec() + ACTION_INTERVAL_MS;
      Act();
    }
  }

  public void Mad(NeedType needType)
  {
    GD.Print("Mad of: ", Enum.GetName(typeof(NeedType), needType));
  }
  public void Act()
  {
    var mostWanted = _needs.OrderByDescending(n => n.Value).Where(n => n.Value >= MIN_NEED_VALUE).ToArray();
    if (mostWanted.Length > 0)
    {
      var rnd = (int)(GD.Randf() * mostWanted.Length);
      _pet.GoForNeed(mostWanted[rnd]);
    }
    else
    {
      GD.Print("no needs");
    }
  }

  public async Task Clear(NeedType needType, float workTime)
  {
    var need = _needs.FirstOrDefault(n => n.Type == needType);
    if (need != null)
    {
      await need.WaitClear(workTime);
    }
  }

  public void OnTenSecTimeOut()
  {
    _pet.Shout();
  }
}

public enum NeedType
{
  Hunger,
  Love,
  Cleanness,
  Defecation,
}