using Godot;

public class Inventory : Node2D
{
  public Food Food { get; private set; }

  private Player _player;
  private Node2D _foodItem;

  public override void _Ready()
  {
    _player = GetParent<Player>();
    _foodItem = GetNode<Node2D>("Food");
    _foodItem.Hide();
  }

  public void TakeFood(Food food)
  {
    if (food != null)
    {
      ClearFood();
    }
    Food = food;
    _foodItem.Show();
  }
  public void ClearFood()
  {
    Food = null;
    _foodItem.Hide();
  }
}
public enum FoodType
{
  Dry,
  Can,
}

public class Food
{
  public FoodType Type;
  public float Happiness;
  public Food(FoodType type, float happiness)
  {
    Type = type;
    Happiness = happiness;
  }
}