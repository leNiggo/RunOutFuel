using Godot;
using System;

public class Camera : SpringArm
{
    public override void _Ready()
    {
        ClippedCamera clippedCamera = GetNode<ClippedCamera>("ClippedCamera");
        clippedCamera.AddException(GetParent());
    }


    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ESC"))
        {
            Input.SetMouseMode(Input.GetMouseMode() == Input.MouseMode.Captured
                ? Input.MouseMode.Visible
                : Input.MouseMode.Captured);
        }
        if (Input.GetMouseMode() != Input.MouseMode.Captured) return;
        if (!(@event is InputEventMouseMotion motion)) return;

        var newX = Mathf.Clamp((RotationDegrees.x) - motion.Relative.y * 0.1f, -45f, 45f);
        var newY = RotationDegrees.y - motion.Relative.x * 0.1f;


        RotationDegrees = new Vector3(newX, newY, Rotation.z);

    }
    
}
