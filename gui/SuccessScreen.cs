using System;
using Godot;

public class SuccessScreen : EndScreen
{
  [Export] NodePath TimeNode;
  private Label _timeLabel;
  public override void _Ready()
  {
    base._Ready();
    _timeLabel = GetNode<Label>(TimeNode);
  }
  public new void Show()
  {
    base.Show();
    _timeLabel.Text = TimeSpan.FromSeconds(Global.Instance.TimeLeft).ToString("mm\\:ss");
  }
}