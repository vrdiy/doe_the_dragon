using Godot;
using System;

public partial class ufo : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.

	private player player = null;
	private int speed = 800;
	private Vector2 velocity = new Vector2();
	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player != null)
		{
			var abovePlayer = player.Position.Y - 200;
			var newPos = new Vector2(player.Position.X,abovePlayer);
			
			if (Position.DistanceTo(newPos) > 300)
			{
				velocity = Position.DirectionTo(newPos) * speed;
			}
			velocity += velocity/5;
			Position = newPos;
		}
		
	}

	public void SetTarget(player Player)
	{
		player = Player;
	}
}
