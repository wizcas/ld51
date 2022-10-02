using Godot;

public class Global : Node
{
  public static Global Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<Global>("Global");

  [Export] public float TotalTime = 600;

  public float TimeLeft;

  public ArrowMarker Arrow { get; private set; }
  public Player Player;
  public Pet Pet;
  public GlobalTimer TenSec;
  public CameraShake Camera;
  public HUD HUD;
  public Meow Meow;

  public override void _EnterTree()
  {
    base._EnterTree();
    Arrow = GetNode<ArrowMarker>("ArrowMarker");
    TenSec = GetNode<GlobalTimer>("10s");
    HUD = GetNode<HUD>("GameGUI/HUD");
    Meow = GetNode<Meow>("GameGUI/Meow");
  }

  public override void _Ready()
  {
    base._Ready();
    StartGame();
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    if (TimeLeft > 0)
      TimeLeft = Mathf.Max(TimeLeft - delta, 0);
  }

  public void StartGame()
  {
    TimeLeft = TotalTime;
  }

  public void GameOver(Ending ending)
  {
    GD.Print("Game Over: ", System.Enum.GetName(typeof(Ending), ending));
    TenSec.WaitTime = 0;
    TenSec.Stop();
    GetTree().Paused = true;
  }
}

public enum Ending
{
  None = 0,
  Success,
  Crazy,
  MissDeadline,
}