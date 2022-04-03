using Godot;
using System;
using System.Net;

public class Car : VehicleBody
{

    [Export] public int HorsePower = 200;
    [Export] public int AccellSpeed = 20;

    [Export] public float SteerAngle = Mathf.Deg2Rad(30); 
    [Export] public int SteerSpeed = 3;

    [Export] public int BrakePower = 40;
    [Export] public int BrakeSpeed = 40;

    [Export] public int MaxFuel = 100;
    [Export] public float FuelConsumption = 1;
    
    [Export] public int CostPerLiter = 1;
    [Export] public int CostIncrease = 1;

    
    [Export] public int Cash = 1000;
    [Export] public int InitScore;
    
    public float Fuel;

    [Signal]
    public delegate void FuelChanged(float fuel);

    [Signal]
    public delegate void CashChanged(int cash);

    [Signal]
    public delegate void ScoreChanged(int score);

    [Signal]
    public delegate void FuelUpStatusChanged(bool status);
    
    private bool _isNearPumpStation;
    private bool _hasFueldUp;

    public override void _Ready()
    {
        var guiNode = GetNode<MarginContainer>("../GUI");
        Fuel = MaxFuel;
        Connect(nameof(FuelChanged), guiNode, "OnCarFuelChanged");
        Connect(nameof(CashChanged), guiNode, "OnCashChanged");
        Connect(nameof(ScoreChanged), guiNode, "OnScoreChanged");
        Connect(nameof(FuelUpStatusChanged), guiNode, "OnFuelUpStatusChanged");
        
        EmitSignal(nameof(CashChanged), Cash);
        EmitSignal(nameof(FuelUpStatusChanged), _isNearPumpStation);
    }

  public override void _Process(float delta)
  {
      var throatInput = -Input.GetActionStrength("forward") + Input.GetActionStrength("backward");
      if (EngineForce < -2f)
      {
          Fuel -= FuelConsumption * 0.01f;
          EmitSignal(nameof(FuelChanged), Fuel);
      }
      EngineForce = Fuel > 0f ? Mathf.Lerp(EngineForce, throatInput * HorsePower, AccellSpeed * delta) : 0;
      
      var steerInput = -Input.GetActionStrength("right") + Input.GetActionStrength("left");
      Steering = Mathf.LerpAngle(Steering, steerInput * SteerAngle, SteerSpeed * delta);

      var breakInput = Input.GetActionStrength("break");
      Brake = Mathf.Lerp(Brake, breakInput * BrakePower, BrakeSpeed * delta);

      if (Input.IsActionPressed("fuelUp") && _isNearPumpStation)
      {
          FuelUp(1);
      }
      
  }

  public void OnPumpEnter()
  {
      _isNearPumpStation = true;
      EmitSignal(nameof(FuelUpStatusChanged), _isNearPumpStation);
  }

  public void OnPumpLeave()
  {
      _isNearPumpStation = false;
      EmitSignal(nameof(FuelUpStatusChanged), _isNearPumpStation);
      if (_hasFueldUp)
      {
          CostPerLiter += CostIncrease;
      }
  }

  public void FuelUp(int fuel)
  {
      if (!_hasFueldUp)
      {
          _hasFueldUp = true;
          
      }
      var roundedFuel = Mathf.Round(Fuel);
      if (roundedFuel >= MaxFuel ) {return;}
      if (Cash <= 0) {return;}
      if (Cash < CostPerLiter) {return;}
      Cash -= CostPerLiter;
      Fuel += fuel;
      EmitSignal(nameof(CashChanged), Cash);
      EmitSignal(nameof(FuelChanged), Fuel);


  }
  
}
