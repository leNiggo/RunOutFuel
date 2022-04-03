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
    [Export] public int IncomeCash = 100;
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

    [Signal]
    public delegate void DeliveredCounterChanged(int count);

    [Signal]
    public delegate void GameOver(); 
    
    
    private bool _isNearPumpStation;
    private bool _hasFueldUp;

    private bool _hasCasePickedUp;
    private int _deliverdCounter;

    public override void _Ready()
    {
        var guiNode = GetNode<MarginContainer>("../GUI");
        Fuel = MaxFuel;
        Connect(nameof(FuelChanged), guiNode, "OnCarFuelChanged");
        Connect(nameof(CashChanged), guiNode, "OnCashChanged");
        Connect(nameof(ScoreChanged), guiNode, "OnScoreChanged");
        Connect(nameof(FuelUpStatusChanged), guiNode, "OnFuelUpStatusChanged");
        Connect(nameof(DeliveredCounterChanged), guiNode, "OnDeliveredCounterChanged");
        
        EmitSignal(nameof(CashChanged), Cash);
        EmitSignal(nameof(FuelUpStatusChanged), _isNearPumpStation);
    }

  public override void _Process(float delta)
  {
      var throatInput = -Input.GetActionStrength("forward") + Input.GetActionStrength("backward");
      GD.Print(EngineForce);
      if (EngineForce != 0)
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

      if (Input.IsActionJustPressed("reset") && Cash > 200 && Fuel > 0)
      {
          Translation = new Vector3(80, 2, -100);
          Rotation = Vector3.Zero;
          
          Cash -= 200;
          EmitSignal(nameof(CashChanged), Cash);
      }
      
  }

  public void YouLoose()
  {
      EmitSignal(nameof(GameOver));
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

  public bool HasPickedUpCase()
  {
      return _hasCasePickedUp;
  }
  
  public void PickedUpCase()
  {
     var suitCaseNode = GD.Load<PackedScene>("res://scenes/suitcase/suitcase.tscn");
     AddChild(suitCaseNode.Instance());
     var suitCase = GetNode<Suitcase>("suitcase");

     suitCase.Translation = new Vector3(0, 2, 0);
    
      _hasCasePickedUp = true;
  }

  public void DropCase()
  {
      _hasCasePickedUp = false;
      var suitcase = GetNode<Suitcase>("suitcase");
      RemoveChild(suitcase);
      Cash += IncomeCash;
      EmitSignal(nameof(CashChanged), Cash);
      _deliverdCounter++;
      EmitSignal(nameof(DeliveredCounterChanged), _deliverdCounter);
  }
  
}
