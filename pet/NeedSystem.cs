using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

public class NeedSystem : Node
{
  [Signal] public delegate void HappinessChanged(int happiness);
  internal const int MIN_NEED_VALUE = 30;
  internal const int MAX_NEED_VALUE = 100;

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
  public NeedData MostWanted { get; private set; }

  private List<NeedData> _needs = new List<NeedData>{
    new NeedData(NeedType.Hunger){Increment=5,BoostChance=.3f,HappinessDamage=10},
    new NeedData(NeedType.Love){Increment=10,BoostChance=.7f,HappinessDamage=5},
    new NeedData(NeedType.Defecation){Increment=3,BoostChance=.2f,HappinessDamage=20},
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
      Happiness -= (int)need.HappinessDamage;
    }
    await _pet.Shout();
  }
  public async void Act()
  {
    // won't go for Love needs because there's no such POI.
    // Player needs to comfort the pet manually.
    var urgentNeeds = _needs.Where(n => n.Type != NeedType.Love).OrderByDescending(n => n.Value).Where(n => n.IsUnbearable).ToArray();
    if (urgentNeeds.Length > 0)
    {
      // var rnd = (int)(GD.Randf() * mostWanted.Length);
      var mostWanted = urgentNeeds[0];
      _pet.FulfillNeed(mostWanted);
      MostWanted = mostWanted;
    }
    else
    {
      var love = GetNeed(NeedType.Love);
      if (love.IsUnbearable)
      {
        await Mad(new[] { love });
      }
    }
  }

  public async Task Clear(NeedType needType, float workTime)
  {
    var need = _needs.FirstOrDefault(n => n.Type == needType);
    if (need != null)
    {
      await Task.Delay(TimeSpan.FromSeconds(workTime));
      need.Clear();
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
  Hunger,
  Love,
  Defecation,
}