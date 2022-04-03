using Godot;
using System;

public class Suitcase : Spatial
{
    [Export] public int RotationSpeed = 1;
    public override void _Ready()
    {
        
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
    Rotate(Vector3.Up, Mathf.Pi * RotationSpeed * delta);
  }
}
