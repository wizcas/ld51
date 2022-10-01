using System;
using System.Threading.Tasks;
using Godot;

public class POI : Node2D
{
  #region Nested
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  [Export] public float WorkTime = 2;
  protected Node2D _petAttach;
  protected Node2D _slaveAttach;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    /* Connect("input_event", this,nameof(OnInputEvent)); */
    this.Connect("body_entered", this, nameof(OnBodyEntered));
    _petAttach = GetNodeOrNull<Node2D>("PetAttach");
    _slaveAttach = GetNodeOrNull<Node2D>("SlaveAttach");
  }

  #endregion

  #region Methods
  public Vector2 GetDestination(Creature creature)
  {
    Vector2 pos;
    if (creature is Pet && _petAttach != null)
    {
      pos = _petAttach.GlobalPosition;
    }
    else if (creature is Player && _slaveAttach != null)
    {
      pos = _slaveAttach.GlobalPosition;
    }
    else
    {
      pos = GlobalPosition;
    }
    return pos;
  }
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
  private async void OnBodyEntered(Node body)
  {
    if (body is Creature creature)
    {
      var tasks = new Task[]{
        Work(creature),
        null
      };
      if (body is Player player)
      {
        tasks[1] = player.Interact(this);
      }
      else if (body is Pet pet)
      {
        tasks[1] = pet.Interact(this);
      }
      await Task.WhenAll(tasks);
    }
  }

  protected virtual Task Work(Creature worker) { return Task.CompletedTask; }

  #endregion
}