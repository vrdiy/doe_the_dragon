using Godot;
using System;

public partial class bounce_pad : StaticBody2D
{
	private Vector2 _padStartPos;

	private float _padProgress = 0f;
	private float _padDistance = 300f;
	// Time in seconds for pad to fully traverse its path
	private float _padTime = 0.3f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//if (_padProgress == 0)
		//{
		//	_padStartPos = Position;
		//}
		//if (_padProgress!= 1){
		//	_padProgress += (float)delta/_padTime;
		//	_padProgress = Math.Clamp(_padProgress,0,1);
		//	Position = new Vector2(Position.X,_padStartPos.Y - _padProgress*_padDistance);
		//}
		
	}
}
