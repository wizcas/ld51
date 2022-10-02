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

  protected virtual bool CanPlayerUse => true;
  protected virtual bool CanPetUse => true;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    Connect("input_event", this, nameof(OnInputEvent));
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
      GD.Print("slave attach: ", _slaveAttach.GetPath().ToString());
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
        Global.Instance.Player.SetTargetPOI(this);
      }
    }
  }
  private async void OnBodyEntered(Node body)
  {
    if (body is Creature creature)
    {
      var tasks = new List<Task>();
      if (body is Player player && IsForPlayer && player.IsCurrentPOI(this))
      {
        if (CanPlayerUse)
        {
          tasks.Add(PlayerEnter(player));
          tasks.Add(player.Interact(this));
        }
        else
        {
          // reset player's POI state if it's not usable
          // so that player can continue move.
          player.SetTargetPOI(null);
        }
      }
      else if (body is Pet pet && IsForPet && CanPetUse)
      {
        tasks.Add(PetEnter(pet));
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
        PlayerLeave(player);
      }
      else if (body is Pet pet && IsForPet)
      {
        PetLeave(pet);
      }
    }
  }

  protected virtual Task PlayerEnter(Player player) { return Task.CompletedTask; }
  protected virtual Task PetEnter(Pet pet) { return Task.CompletedTask; }
  protected virtual void PlayerLeave(Player leave) { }
  protected virtual void PetLeave(Pet pet) { }

  #endregion
}