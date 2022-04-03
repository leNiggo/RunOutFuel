using Godot;
using System;
using System.Runtime.InteropServices;

public class SuitCasesPlaces : Spatial
{

    private Area _areaANode; 
    private Area _areaBNode;

    private PackedScene _caseScene;
    
    [Signal]
    public delegate void CasePickedUp();

    [Signal]
    public delegate void CaseDeliverd();
    

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _areaANode = GetNode<Area>("SuitCaseA");
        _areaBNode = GetNode<Area>("SuitCaseB");
        _caseScene = GD.Load<PackedScene>("res://scenes/suitcase/suitcase.tscn");
        
        var node = _caseScene.Instance();

        var child = _areaANode.GetChild<CollisionShape>(0);
        
        child.AddChild(node);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void OnSuitCasePickUp(Node node)
    {
        if (!(node is Car car))
        {
            return;
        }

        if (car.HasPickedUpCase())
        {
            return;
        }
        var child = _areaANode.GetChild<CollisionShape>(0);
        var suitCase = child.GetChild<Suitcase>(0);
        suitCase.Hide();
        car.PickedUpCase();
            
    }

    public void OnCaseDroped(Node node)
    {
        if (!(node is Car car))
        {
            return;
        }

        if (car.HasPickedUpCase())
        {
            GD.Print("Case deliverd");
            var child = _areaANode.GetChild<CollisionShape>(0);
            var suitCase = child.GetChild<Suitcase>(0);
            car.DropCase();
            suitCase.Show();
        }
    }
}
