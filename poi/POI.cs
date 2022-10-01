using System;
using System.Collections.Generic;
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
  [Export] public bool IsForPlayer = true;
  [Export] public bool IsForPet = true;
  protected Node2D _petAttach;
  protected Node2D _slaveAttach;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    /* Connect("input_event", this,nameof(OnInputEvent)); */
    this.Connect("body_entered", this, nameof(OnBodyEntered));
    this.Connect("body_exited", this, nameof(OnBodyExited));
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
      var tasks = new List<Task>();
      if (body is Player player && IsForPlayer)
      {
        tasks.Add(Enter(creature));
        tasks.Add(player.Interact(this));
      }
      else if (body is Pet pet && IsForPet)
      {
        tasks.Add(Enter(creature));
        tasks.Add(pet.Interact(this));
      }
      await Task.WhenAll(tasks);
    }
  }

  private void OnBodyExited(Node body)
  {
    if (body is Creature creature)
    {
      if (body is Player player && IsForPlayer)
      {
        Leave(creature);
      }
      else if (body is Pet pet && IsForPet)
      {
        Leave(creature);
      }
    }
  }

  protected virtual Task Enter(Creature worker) { return Task.CompletedTask; }
  protected virtual void Leave(Creature worker) { }

  #endregion
}