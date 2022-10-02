using System;
using Godot;

public class GameInfo : Control
{
  #region Nested
  #endregion

  #region Signals
  #endregion

  #region Fields & Properties
  [Export] public NodePath DeadLineNode;
  [Export] public NodePath WorkProgressNode;
  private TextureProgress _workProgress;
  private Label _deadline;
  private SanitySystem Sanity => Global.Instance.Player?.Sanity;
  #endregion

  #region Hooks
  public override void _Ready()
  {
    if (!WorkProgressNode.IsEmpty())
      _workProgress = GetNode<TextureProgress>(WorkProgressNode);
    if (!DeadLineNode.IsEmpty())
      _deadline = GetNode<Label>(DeadLineNode);
  }

  public override void _Process(float delta)
  {
    if (Sanity != null)
    {
      _workProgress.MaxValue = Sanity.MaxWorkProgress;
      _workProgress.Value = Sanity.WorkProgress;
    }
    if (_deadline != null)
    {
      _deadline.Text = FormatTime(Global.Instance.TimeLeft);
    }
  }
  #endregion

  #region Methods
  private string FormatTime(float time)
  {
    var timeSpan = TimeSpan.FromSeconds(time);
    return timeSpan.ToString("mm\\:ss");
  }
  #endregion
}