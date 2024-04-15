using Godot;
using System;

public partial class doublejump : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("DeletionTimer").Start();
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Delete()
	{
		QueueFree();
		GD.Print("Deleted Double jump");
	}
}
