using Godot;
using System;
using System.Globalization;

public class FuelCounter : MarginContainer
{
	private Label _numberLabel;
	private Label _cashLabel;
	private MarginContainer _fuelUpText;
	private Label _deliveredCounter;

	public override void _Ready()
	{
		const string rootPath = "HBoxContainer/Bars/";
		_numberLabel = GetNode<Label>(rootPath + "Bar/Count/Number");
		_cashLabel = GetNode<Label>(rootPath + "CashBar/Cash/Label");
		_fuelUpText = GetNode<MarginContainer>(rootPath + "fuelUp/fuelUpContainer");
		_deliveredCounter = GetNode<Label>(rootPath + "DeliveredCounter/MarginContainer/Label");
	}
	
	public void OnCarFuelChanged(float fuel)
	{
		var roundedFuel = Mathf.Round(fuel);
		_numberLabel.Text = roundedFuel.ToString(CultureInfo.InvariantCulture);
	}

	public void OnCashChanged(int cash)
	{
		var cashString = cash + " EUR";
		_cashLabel.Text = cashString;
	}

	public void OnFuelUpStatusChanged(bool status)
	{
		_fuelUpText.Visible = status;
	}

	public void OnDeliveredCounterChanged(int counter)
	{
		_deliveredCounter.Text = "Delivered: " + counter;
	}

	public void OnGameOver()
	{
		
	}
}
