using Godot;

public class Global : Node
{
  public static Global Instance => ((SceneTree)Engine.GetMainLoop()).Root.GetNode<Global>("Global");

  public ArrowMarker Arrow { get; private set; }
  public Player Player;

  public override void _Ready()
  {
    base._Ready();
    Arrow = GetNode<ArrowMarker>("ArrowMarker");
  }
}