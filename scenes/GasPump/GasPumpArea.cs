using Godot;
using System;

public class GasPumpArea : Area
{

    [Export] public int CostPerLiter = 1;
    
    public void OnEnter(Node node)
    {
        GD.Print(node.Name);
        if (!(node is Car car)) {return;}
        GD.Print("On Pump enter...!");
        
        car.OnPumpEnter();

    }

    public void OnLeave(Node node)
    {
        if (!(node is Car car)) {return;}
        
        car.OnPumpLeave();

    }
    

}
