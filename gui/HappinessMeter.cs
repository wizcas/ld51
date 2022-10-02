using System;
using System.Linq;
using Godot;

public class HappinessMeter : TextureRect
{
  #region Nested
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  [Export] public Texture[] Textures;
  [Export] public HappinessType HappinessType;

  public HappinessRange[] Ranges = new[]{
    new HappinessRange{Type=HappinessType.Angry,  Min=0, Max=20},
    new HappinessRange{Type=HappinessType.Sad,  Min=21, Max=50},
    new HappinessRange{Type=HappinessType.Normal,  Min=51, Max=80},
    new HappinessRange{Type=HappinessType.Normal,  Min=81, Max=100},
  };

  #endregion

  #region Hooks
  public override void _EnterTree()
  {
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    if (Global.Instance.Pet != null)
    {
      UpdateMeter((int)Global.Instance.Pet.Needs.Happiness);
    }
  }
  #endregion

  #region Methods
  private void UpdateMeter(int happiness)
  {
    if (Ranges == null) return;

    var textureIndex = Ranges.FirstOrDefault(r => r.InRange(happiness))?.TextureIndex ?? 0;
    Texture = Textures[textureIndex];
  }

  #endregion
}

public class HappinessRange : Godot.Object
{
  public int Min;
  public int Max;
  public HappinessType Type;
  public int TextureIndex => (int)Type;

  public bool InRange(int happiness)
  {
    return happiness >= Min && happiness <= Max;
  }
}

public enum HappinessType
{
  Normal,
  Happy,
  Sad,
  Angry
}