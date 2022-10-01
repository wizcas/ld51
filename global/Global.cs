using Godot;

public class Global : Node
{
  public static Global Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<Global>("Global");

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

  public void GameOver(Ending ending)
  {
    GD.Print("Game Over: ", System.Enum.GetName(typeof(Ending), ending));
  }
}

public enum Ending
{
  None = 0,
  Success,
  Crazy,
  MissDeadline,
}