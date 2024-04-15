using Godot;
using System;

public partial class goal : Area2D
{
	
	[Signal]
	public delegate void GoalGetEventHandler();

	private bool has_been_collected = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_area_entered(Node2D node)
	{
		if (node.GetParent() is player && !has_been_collected)
		{
			has_been_collected = true;
			player p = (player)node.GetParent();
			EmitSignal(SignalName.GoalGet);
			//p.SetGoal(null);
			QueueFree();
		}
		
	}
}
