using System;
using System.Threading.Tasks;
using Godot;

public class Global : Node
{
  public static Global Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<Global>("Global");

  [Signal] public delegate void GameReset();
  [Export] public float TotalTime = 300;

  public float TimeLeft;

  public ArrowMarker Arrow { get; private set; }
  public Player Player;
  public Pet Pet;
  public GlobalTimer TenSec;
  public CameraShake Camera;
  public HUD HUD;
  public Meow Meow;
  public StartScreen StartScreen;
  public SuccessScreen SuccessScreen;
  public Control CrazyEndScreen;
  public Control FailedEndScreen;

  public readonly Vector2 PlayerSpawn = new Vector2(148, 95);
  public readonly Vector2 PetSpawn = new Vector2(152, 119);

  private Control[] EndScreens => new[] { SuccessScreen, CrazyEndScreen, FailedEndScreen };
  private AudioStreamPlayer _bgm;

  public override void _EnterTree()
  {
    base._EnterTree();
    Arrow = GetNode<ArrowMarker>("ArrowMarker");
    TenSec = GetNode<GlobalTimer>("10s");
    HUD = GetNode<HUD>("GameGUI/HUD");
    Meow = GetNode<Meow>("GameGUI/Meow");
    StartScreen = GetNode<StartScreen>("GameGUI/StartScreen");
    SuccessScreen = GetNode<SuccessScreen>("GameGUI/EndingSuccess");
    CrazyEndScreen = GetNode<Control>("GameGUI/EndingCrazy");
    FailedEndScreen = GetNode<Control>("GameGUI/EndingFailed");
    _bgm = GetNode<AudioStreamPlayer>("BGM");
  }

  public override void _Ready()
  {
    base._Ready();

    foreach (Control endScreen in EndScreens)
    {
      endScreen.GetNodeOrNull("RestartButton")?.Connect("pressed", this, nameof(Restart));
    }

    GetTree().Paused = true;
    HideEndScreens();
    StartScreen.Show();
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    if (TimeLeft > 0)
    {
      TimeLeft = Mathf.Max(TimeLeft - delta, 0);
      if (TimeLeft < 10)
      {
        _bgm.PitchScale = 1.5f;
      }
      else if (TimeLeft < 60)
      {
        _bgm.PitchScale = 1.2f;
      }
      else
      {
        _bgm.PitchScale = 1;
      }

    }
    else
    {
      GameOver(Ending.MissDeadline);
    }
  }

  public void StartGame()
  {
    TimeLeft = TotalTime;
    EmitSignal(nameof(GameReset));
    StartScreen.Hide();
    GetTree().Paused = false;
    TenSec.Start();
    _bgm.Seek(0);
    _bgm.Play();
  }

  public async void Restart()
  {
    _bgm.Stop();
    GetTree().Paused = true;
    await Task.Delay(TimeSpan.FromSeconds(.5));
    HideEndScreens();
    StartScreen.Show();
  }

  public void GameOver(Ending ending)
  {
    _bgm.Stop();
    GD.Print("Game Over: ", System.Enum.GetName(typeof(Ending), ending));
    TenSec.Stop();
    GetTree().Paused = true;
    switch (ending)
    {
      case Ending.Success:
        SuccessScreen.Show();
        break;
      case Ending.Crazy:
        CrazyEndScreen.Show();
        break;
      case Ending.MissDeadline:
      default:
        FailedEndScreen.Show();
        break;
    }
  }

  private void HideEndScreens()
  {
    foreach (Control endScreen in EndScreens)
    {
      endScreen?.Hide();
    }
  }
}

public enum Ending
{
  None = 0,
  Success,
  Crazy,
  MissDeadline,
}