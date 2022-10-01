using System;
using System.Threading.Tasks;
using Godot;

public class Meow : Label
{
  #region Nested
  [Export] public float Margin = 16f;
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  private Node2D _origin;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    Hide();
  }
  public override void _Process(float delta)
  {
    base._Process(delta);
    if (_origin == null) return;

    var showVPos = _origin.GetViewportTransform() * _origin.GlobalPosition + Vector2.Up * Margin;
    var vp = GetViewportRect();
    var center = vp.Size / 2;
    showVPos.x = Mathf.Clamp(showVPos.x, 0, vp.Size.x - Margin);
    showVPos.y = Mathf.Clamp(showVPos.y, 0, vp.Size.y - Margin);
    RectGlobalPosition = showVPos;
    UpdateRotation(showVPos, center);
  }
  #endregion

  #region Methods
  public async void Pop(Node2D origin)
  {
    _origin = origin;
    Show();
    await Task.Delay(1000);
    Hide();
  }

  private void UpdateRotation(Vector2 pos, Vector2 center)
  {
    if (pos.x <= center.x)
    {
      RectRotation = Mathf.Rad2Deg((center - pos).Normalized().Angle());
      RectPivotOffset = new Vector2(0, RectSize.y / 2);
    }
    else
    {
      RectRotation = Mathf.Rad2Deg((pos - center).Normalized().Angle());
      RectPivotOffset = new Vector2(RectSize.x, RectSize.y / 2);
    }
  }
  #endregion
}