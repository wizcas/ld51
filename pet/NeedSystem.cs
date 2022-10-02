using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class NeedSystem : Node
{
  [Signal] public delegate void HappinessChanged(int happiness);
  const int MIN_NEED_VALUE = 30;
  const int MAX_NEED_VALUE = 100;

  private float _happiness = 100;
  public float Happiness
  {
    get => _happiness;
    set
    {
      _happiness = Mathf.Clamp(value, 0, 100);
      EmitSignal(nameof(HappinessChanged), (int)_happiness);
    }
  }

  private List<NeedData> _needs = new List<NeedData>{
    new NeedData(NeedType.Hunger){Rate=3},
    new NeedData(NeedType.Love){Rate=2,BoostChance=.7f},
    new NeedData(NeedType.Defecation){Rate=3,Increment=10},
  };
  private Pet _pet;

  public override void _Ready()
  {
    base._Ready();
    _pet = GetParent<Pet>();
    Global.Instance.TenSec.Connect("timeout", this, nameof(OnTenSecTimeOut));
    Happiness = 100;
  }

  public override void _Process(float delta)
  {
    base._Process(delta);

    foreach (var need in _needs)
    {
      need.Process(delta);
    }
  }

  public async Task Mad(IEnumerable<NeedData> needs)
  {
    if (needs == null || needs.Count() <= 0) return;
    foreach (var need in needs)
    {
      Happiness -= (int)need.Type;
    }
    await _pet.Shout();
  }
  public async void Act()
  {
    var madNeeds = _needs.Where(n => n.Value >= MAX_NEED_VALUE);
    await Mad(madNeeds);

    var mostWanted = _needs.OrderByDescending(n => n.Value).Where(n => n.Value >= MIN_NEED_VALUE).ToArray();
    if (mostWanted.Length > 0)
    {
      var rnd = (int)(GD.Randf() * mostWanted.Length);
      _pet.FulfillNeed(mostWanted[rnd]);
    }
    else
    {
      GD.Print("no needs");
      // TODO: randomly ask for love
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
    Act();
  }

  public NeedData GetNeed(NeedType type)
  {
    return _needs.FirstOrDefault(n => n.Type == type);
  }
}

public enum NeedType
{
  Hunger = 10,
  Love = 2,
  Defecation = 20,
}