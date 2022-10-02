using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public class Pet : Creature
{
  #region Fields and Properties
  [Signal] public delegate void Shouting(Pet pet);
  [Export] NodePath WanderingAreaNode;
  [Export] public float ShoutTime = .5f;
  [Export] public AudioStreamMP3[] ShoutSounds;
  [Export] public AudioStreamMP3 EatSound;
  [Export] public AudioStreamMP3 PurrSound;

  public NeedSystem Needs { get; private set; }
  private WanderingArea _wanderingArea;
  private float _nextActionTime;
  private CPUParticles2D _loveFX;
  private Sprite _sprite;
  #endregion

  #region Hooks
  public override void _EnterTree()
  {
    base._EnterTree();
    Global.Instance.Pet = this;
  }
  public override void _Ready()
  {
    base._Ready();
    _sprite = GetNode<Sprite>("Sprite");
    Needs = GetNode<NeedSystem>("NeedSystem");
    _loveFX = GetNode<CPUParticles2D>("LoveFX");
    if (!WanderingAreaNode.IsEmpty())
    {
      _wanderingArea = GetNode<WanderingArea>(WanderingAreaNode);
    }
  }
  public override void OnGameReset()
  {
    base.OnGameReset();
    _loveFX.Emitting = false;
    _nextActionTime = 0;
    GlobalPosition = Global.Instance.PetSpawn;
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    _sprite.FlipH = _velocity.x > 0;
    if (_wanderingArea != null && !_isNavigating && !_isFrozen && !IsBusy && OS.GetTicksMsec() > _nextActionTime)
    {
      var location = _wanderingArea.GetRandomLocation();
      if (location.HasValue)
      {
        SetNavTarget(location.Value);
        location = null;
      }
    }
  }
  #endregion

  #region Methods
  public void FulfillNeed(NeedData need)
  {
    var nodes = GetTree().GetNodesInGroup("pet-needs");
    var candidates = new List<PetPOI>();
    foreach (Node node in nodes)
    {
      if (node is PetPOI poi && poi.Type == need.Type)
      {
        candidates.Add(poi);
      }
    }
    if (candidates.Count == 0)
    {
      GD.PrintErr("cannot meet pet's need for no poi found: ", need.Name);
    }
    else
    {
      var rnd = (int)(GD.Randf() * candidates.Count);
      var poi = candidates[rnd];
      SetNavTarget(poi.GetDestination(this));
      Think();
    }
  }
  public async override Task Interact(POI poi)
  {
    IsBusy = true;
    Stop();
    ForceMoveTo(poi.GetDestination(this));
    await Task.Delay(TimeSpan.FromSeconds(poi.WorkTime));
    CancelForceMove();
    IsBusy = false;
  }
  public async Task Disappoint()
  {
    if (Needs.MostWanted != null && Needs.MostWanted.IsUnbearable)
    {
      await Needs.Mad(new[] { Needs.MostWanted });
    }
  }
  public async Task Shout()
  {
    IsBusy = true;
    Stop();
    EmitSignal(nameof(Shouting), this);
    PlaySound(PetSound.Shout);
    await Task.Delay(TimeSpan.FromSeconds(ShoutTime));
    Think();
    IsBusy = false;
  }
  public override void Stop()
  {
    base.Stop();
    Think();
  }

  public void Think()
  {
    if (_isNavigating) return;
    _nextActionTime = OS.GetTicksMsec() + 1000;
  }

  public void PlaySound(PetSound sound)
  {
    var audio = GetNode<AudioStreamPlayer2D>("Sound");
    AudioStreamMP3 stream = null;
    switch (sound)
    {
      case PetSound.Eat:
        stream = EatSound;
        break;
      case PetSound.Purr:
        stream = PurrSound;
        break;
      case PetSound.Shout:
        stream = ShoutSounds[(int)(GD.Randf() * ShoutSounds.Length)];
        break;
    }
    if (stream != null)
    {
      var length = stream.GetLength();
      audio.Stop();
      audio.Stream = stream;
      audio.Seek(0);
      audio.Play();
    }
  }

  public void Love(bool on)
  {
    _loveFX.Emitting = on;
  }
  #endregion
}

public enum PetSound
{
  None,
  Shout,
  Eat,
  Purr,
  Poop,
}