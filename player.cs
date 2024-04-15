using Godot;
using System;

public partial class player : CharacterBody2D
{

	[Signal]
	public delegate void SummonPillarEventHandler();

	[Signal]
	public delegate void SummonEventHandler();
	[Signal]
	public delegate void DiedEventHandler();

	[Export]
	public PackedScene DoubleJump {get; set;}
	// Called when the node enters the scene tree for the first time.

	//[Export]
	//public int _pillarCharges {get; set;} = 5;

	[Export]
	public int _playerWalkSpeed {get; set;} = 12;

	[Export]
	public int _playerGravity {get; set;} = 3200;

	[Export]
	public int _playerSpeedRatio {get; set;} = 5;

	[Export]
	public int _maxSpeed {get; set;} = 500;
	private Vector2 velocity = new Vector2();
	private Vector2 _screenSize;
	private int _jumpHeight = 900;
	private bool _canWallJump = false;
	private bool _canDoubleJump = false;
	private bool _isWallSliding = false;
	private pillar _prevPillar = null;
	private Node2D _goal = null;
	public int numCharges {get;set;}= 5;
	private int _wallJumpDirection = 1; // 1=right -1=left
	public override void _Ready()
	{
		_screenSize = DisplayServer.ScreenGetSize();
	}
	public void PlayJumpSound(float pitch = 1)
	{
		var asp = GetNode<AudioStreamPlayer2D>("Jump");
		asp.PitchScale = pitch;
		asp.Play();

	}

	public void SetGoal(Node2D goal)
	{
		_goal = goal;
	}
	public void UpdateDirection()
	{
		if (_goal != null)
		{
			var nodedir = GetNode<Node2D>("Direction");
			nodedir.LookAt(_goal.Position);
		}
	}
	public void Die()
	{
		EmitSignal(SignalName.Died);
		var asp = GetNode<AudioStreamPlayer2D>("Death");
		asp.Play();

	}
	public void SpawnDoubleJumpSprite()
	{
		doublejump dj = DoubleJump.Instantiate<doublejump>();
		AddChild(dj);
		dj.Position = new Vector2(dj.Position.X,dj.Position.Y+40);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateDirection();
		if (Position.Y>900){
			Die();
		}
		_isWallSliding = false;
		var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		bool onFloor = IsOnFloor();
		bool tookJumpingAction = false;
		
		if (Input.IsActionPressed("left"))
		{
			velocity.X -= _playerWalkSpeed;
		 	animatedSprite.FlipH = true;
			if (_canWallJump && _wallJumpDirection == 1){ // wall slide
				_isWallSliding = true;
			}
		}
		if (Input.IsActionPressed("right"))
		{
			velocity.X += _playerWalkSpeed;
		 	animatedSprite.FlipH = false;
			if (_canWallJump && _wallJumpDirection == -1){ // wall slide
				_isWallSliding = true;
			}

		}
		if (Input.IsActionJustPressed("jump"))
		{
			float pitch = 1;
			if (onFloor)
			{
				velocity.Y -= _jumpHeight;
			}
			else
			{
				if (_canWallJump)
				{
					velocity.Y = -_jumpHeight*0.9f;
					velocity.X = _maxSpeed*1.2f*_wallJumpDirection;
					_canWallJump = false;
					pitch += .4f;
				}
				else if (_canDoubleJump)
				{
					pitch += .8f;
					velocity.Y = -_jumpHeight*0.8f;
					_canDoubleJump = false;
					SpawnDoubleJumpSprite();
				}
			}
			tookJumpingAction = true;
			PlayJumpSound(pitch);

		}
		
		if (Input.IsActionJustPressed("summon_pillar"))
		{
			EmitSignal(SignalName.SummonPillar);
		}
		if (Input.IsActionJustPressed("left_summon"))
		{
			EmitSignal(SignalName.Summon,"left","pillar");
		}
		if (Input.IsActionJustPressed("right_summon"))
		{
			EmitSignal(SignalName.Summon,"right","pillar");
		}
		
		animatedSprite.Play();

		// Apply Less Gravity when jump held
		if (Input.IsActionPressed("jump") && velocity.Y < 0){
			velocity.Y += _playerGravity*0.6f*(float)delta;
			tookJumpingAction = true;
		}
		else
		{
			if (_isWallSliding)
			{
				velocity.Y += _playerGravity*0.3f * (float)delta;
			}
			else
			{
				velocity.Y += _playerGravity * (float)delta;
			}
		}
		
		velocity.X = Math.Clamp(velocity.X,-_maxSpeed,_maxSpeed);
		// Apply limit to X
		int direction = Math.Sign(velocity.X);
		if (Math.Abs(velocity.X) > 10)
		{
			velocity.X -=  _playerSpeedRatio*direction;
		}
		else
		{
			velocity.X = 0;
		}
		

		if (onFloor && Math.Abs(velocity.X) > 20)
		{
			animatedSprite.Animation = "Walk";

		}
		else if (onFloor)
		{
			animatedSprite.Animation = "Idle";
		}
		else if (_canWallJump)
		{
			animatedSprite.Animation = "WallGrab";
		}
		else
		{
			animatedSprite.Animation = "Falling";
		}
		if (onFloor && !tookJumpingAction){
			_prevPillar = null;
			velocity.Y = 0;
			_canDoubleJump = true;
			//GD.Print("Touched floor");
			_canWallJump = false;
		}
		velocity.Y = Math.Clamp(velocity.Y,-1500,1500);
		Position += velocity * (float)delta;
		//GD.Print(velocity);

		MoveAndSlide();
	}
	
	public void OnWallJumpRightDetected(Node2D collided)
	{
		if (collided is pillar || collided is bounce_pad)
		{
			if (_prevPillar == collided){
				return;
			}
			_canWallJump = true;
			//GD.Print("Pillar collided");
			_wallJumpDirection = -1;
			_prevPillar = (pillar)collided;
		}
	}
	public void OnWallJumpLeftDetected(Node2D collided)
	{
		if (collided is pillar || collided is bounce_pad)
		{
			if (_prevPillar == collided){
				return;
			}
			_canWallJump = true;
			//GD.Print("Pillar collided");
			_wallJumpDirection = 1;
			_prevPillar = (pillar)collided;


		}
	}
	public void OnCollideExit(Node2D collided)
	{
		if (collided is pillar || collided is bounce_pad)
		{
			_canWallJump = false;
			GD.Print("Exit");
		}
	}
	
}
