using Godot;

public class Inventory : Node
{
  public Food Food;

  private Player _player;

  public override void _Ready()
  {
    _player = GetParent<Player>();
  }

  public void TakeFood(Food food)
  {
    Food = food;
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