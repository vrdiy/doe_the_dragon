using Godot;
using System;

public partial class spike : Area2D
{
	// Called when the node enters the scene tree for the first time.
	private bool has_killed=false;
	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("Spike").Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_area_entered(Node2D node)
	{
		if (node.GetParent() is player && !has_killed)
		{
			has_killed = true;
			player p = (player)node.GetParent();
			p.Die();
			//p.SetGoal(null);
			//QueueFree();
		}
		
	}
}
