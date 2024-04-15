using Godot;
using System;

public partial class pillar : StaticBody2D
{
	[Signal]
	public delegate void GiveChargeEventHandler(int val);

	private Vector2 _pillarStartPos;

	private float _pillarProgress = 0f;
	private float _pillarDistance = 300f;
	// Time in seconds for pillar to fully traverse its path
	private float _pillarTime = 0.3f;
	private float lifespan = 0.0f;
	public int value = 1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("Death Timer").Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		lifespan += (float)delta;
		Modulate = new Color(Modulate.R,Modulate.G,Modulate.B,1-lifespan/4);
		if (_pillarProgress == 0)
		{
			_pillarStartPos = Position;
		}
		if (_pillarProgress!= 1){
			_pillarProgress += (float)delta/_pillarTime;
			_pillarProgress = Math.Clamp(_pillarProgress,0,1);
			Position = new Vector2(Position.X,_pillarStartPos.Y - _pillarProgress*_pillarDistance);
		}
		
	}

	public void _on_death_timer_timeout()
	{
		EmitSignal(SignalName.GiveCharge,value);
		QueueFree();
	}
}
