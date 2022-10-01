using System;
using Godot;

public class WanderingArea : Polygon2D
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

  }
  #endregion

  #region Methods
  public Vector2? GetRandomLocation()
  {
    var indices = Geometry.TriangulatePolygon(this.Polygon);
    if (indices.Length == 0)
    {
      GD.PrintErr("failed finding random point: No triangles found");
      return null;
    }
    var i0 = (int)(GD.Randf() * indices.Length / 3) * 3;
    var i1 = i0 + 1;
    var i2 = i0 + 2;
    var point = RandomPointInTriangle(this.Polygon[indices[i0]], this.Polygon[indices[i1]], this.Polygon[indices[i2]]);
    return ToGlobal(point);
  }

  private Vector2 RandomPointInTriangle(Vector2 a, Vector2 b, Vector2 c)
  {
    var r1 = GD.Randf();
    var r2 = GD.Randf();
    return (1 - Mathf.Sqrt(r1)) * a + (Mathf.Sqrt(r1) * (1 - r2)) * b + (Mathf.Sqrt(r1) * r2) * c;
  }
  #endregion
}