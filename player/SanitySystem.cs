using System;
using Godot;

public class SanitySystem : Node
{
  [Signal] public delegate void SanityChanged(float sanity, float max);
  [Signal] public delegate void WorkProgressChanged(float progress, float max);
  [Export] public float MaxSanity = 100;
  [Export] public float MaxWorkProgress = 100;

  [Export] public float SanityWorkLossRate = 2f;
  [Export] public float SanityShoutLossRate = 20f;
  [Export] public float SanitySleepGainRate = 10f;
  [Export] public float WorkGainRate = 1;

  public float Sanity;
  public float WorkProgress;
  private Player _player;

  #region Hooks
  public override void _Ready()
  {
    base._Ready();
    _player = GetParent<Player>();
    Global.Instance.Pet.Connect(nameof(Pet.Shouting), this, nameof(OnPetShouting));
    Global.Instance.Connect(nameof(Global.GameReset), this, nameof(OnGameReset));
    StartGame();
  }
  public override void _Process(float delta)
  {
    base._Process(delta);
    switch (_player.CurrentAction)
    {
      case Player.Action.Working:
        Sanity -= SanityWorkLossRate * delta;
        WorkProgress += WorkGainRate * delta;
        break;
      case Player.Action.Sleeping:
        Sanity += SanitySleepGainRate * delta;
        break;
    }
    Sanity = Mathf.Clamp(Sanity, 0, MaxSanity);
    WorkProgress = Mathf.Clamp(WorkProgress, 0, MaxWorkProgress);

    EmitSignal(nameof(SanityChanged), Sanity, MaxSanity);
    EmitSignal(nameof(WorkProgressChanged), WorkProgress, MaxWorkProgress);

    if (WorkProgress >= MaxWorkProgress)
    {
      Global.Instance.GameOver(Ending.Success);
    }
    else if (Sanity <= 0)
    {
      Global.Instance.GameOver(Ending.Crazy);
    }
  }
  #endregion

  #region Methods

  public void StartGame()
  {
    Sanity = MaxSanity;
    WorkProgress = 0;
  }
  public void OnPetShouting(Pet pet)
  {
    Sanity -= SanityShoutLossRate;
  }
  public void OnGameReset()
  {
    StartGame();
  }

  #endregion
}