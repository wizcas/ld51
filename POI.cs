using System;
using Godot;

public class POI : StaticBody2D
{
  #region Nested
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  #endregion

  #region Hooks
  public override void _Ready()
  {
    Connect("input_event", this, nameof(OnInputEvent));
    GetNode("InteractArea").Connect("body_entered", this, nameof(OnBodyEntered));
  }

  #endregion

  #region Methods
  public void OnInputEvent(Node viewport, InputEvent e, int shapeIdx)
  {
    if (e is InputEventMouseButton mouseEvent)
    {
      if (mouseEvent.IsPressed() && mouseEvent.ButtonIndex == (int)ButtonList.Left)
      {
        Global.Instance.Arrow.ShowAt(GlobalPosition);
        GD.Print("POI clicked: ", Name);
        GetTree().SetInputAsHandled();
      }
    }
  }
  private void OnBodyEntered(Node body)
  {
    if (body is Player player)
    {
      player.Interact(this);
    }

    // TODO: if body is pet
  }

  #endregion
}