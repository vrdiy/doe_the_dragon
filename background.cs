using Godot;
using System;

public partial class background : ParallaxBackground
{
	private int _cloudSpeed = 3;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Position = new Vector2(Position.X - (float)Math.Sin(delta*_cloudSpeed),Position.Y);
		
		
	}
}
