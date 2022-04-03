using Godot;
using System;

public class Player : Spatial
{
    [Export] public int Cash = 1000;
    [Export] public int InitScore;

    private bool IsNearPump = false;


    [Signal]
    public delegate void OnCashChanged(int cash);

    [Signal]
    public delegate void OnScoreChanged(int score);
    
    
    public override void _Ready()
    {
        InitScore = 0;
        Connect(nameof(OnCashChanged), this, nameof(OnCashChanged));
        Connect(nameof(OnScoreChanged), this, nameof(OnScoreChanged));
    }

    public void OnPumpEnter()
    {
        IsNearPump = true;
        GD.Print("You can now pump gas");
    }
    
    
    public void OnPumpLeave()
    {
        IsNearPump = false;
        GD.Print("You CANT pump gas");

    }

    public void PumpGas(int cost)
    {
        if (!IsNearPump)
        {
            return;
        }
        
        GD.Print("pump gas...");
    }
    
}
