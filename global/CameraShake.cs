using System;
using Godot;

public class CameraShake : Camera2D
{
  #region Hooks
  private Tween _tween;
  private Timer _freqTimer;
  private Timer _durationTimer;
  private float _amplitude;
  private int _priority;
  public override void _EnterTree()
  {
    base._EnterTree();
    Global.Instance.Camera = this;
  }

  public override void _Ready()
  {
    base._Ready();
    _tween = GetNode<Tween>("ShakeTween");
    _freqTimer = GetNode<Timer>("Frequency");
    _durationTimer = GetNode<Timer>("Duration");

    _freqTimer.Connect("timeout", this, nameof(OnFreqTimeOut));
    _durationTimer.Connect("timeout", this, nameof(OnDurationTimeOut));
    Global.Instance.Pet.Connect(nameof(Pet.Shouting), this, nameof(OnPetShouting));
  }

  #region Methods
  public void StartShaking(float duration = .5f, float frequency = 30f, float amplitude = 2f, int priority = 0)
  {
    if (priority < _priority) return;
    _amplitude = amplitude;
    _priority = priority;

    _durationTimer.Start(duration);
    _freqTimer.Start(1 / frequency);

    NewShake();
  }
  public void NewShake()
  {
    var rnd = new Vector2(
      (float)GD.RandRange(-_amplitude, _amplitude),
      (float)GD.RandRange(-_amplitude, _amplitude)
    );
    _tween.InterpolateProperty(this, "offset", Offset, rnd, _freqTimer.WaitTime, Tween.TransitionType.Sine, Tween.EaseType.InOut);
    _tween.Start();
  }
  public void Reset()
  {
    _tween.InterpolateProperty(this, "offset", Offset, Vector2.Zero, _freqTimer.WaitTime, Tween.TransitionType.Sine, Tween.EaseType.InOut);
    _tween.Start();
    _priority = 0;
  }
  public void OnDurationTimeOut()
  {
    _freqTimer.Stop();
    Reset();
  }

  public void OnFreqTimeOut()
  {
    NewShake();
  }

  public void OnPetShouting(Pet pet)
  {
    StartShaking();
  }
  #endregion
  #endregion
}