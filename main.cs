using Godot;
using GodotPlugins.Game;
using System;

public partial class main : Node2D
{
	[Export]
	public PackedScene PillarScene {get; set;}

	[Export]
	public PackedScene BouncePadScene {get; set;}

	[Export]
	public PackedScene StarScene {get; set;}

	[Export]
	public PackedScene SpikeScene {get; set;}

	private int numCharges = 5;
	private int levelNum = 1;
	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{
		GD.Seed("vrdiy".Hash());
	}
	public void PlayStarGetSound()
	{
		var asp = GetNode<AudioStreamPlayer2D>("StarCollected");
		asp.PitchScale = GD.Randf()+1;
		asp.Play();
	}
	public void SpawnStar(bool first = false)
	{
		goal star = StarScene.Instantiate<goal>();
		
		Vector2 pos = new Vector2(2280,490);

		if (!first)
		{
			pos.X = GD.Randf()*((float)Math.Ceiling((double)levelNum/5)*1500);
			pos.Y = -GD.Randf()*((float)Math.Ceiling((double)levelNum/5)*1500);
			PlayStarGetSound();
		}
		player player = GetNode<player>("Player");
		player.Position = GetNode<Node2D>("SpawnPos").Position;
		star.Position = pos;
		var signal = star.GetSignalList();
		star.GoalGet += GoalGot;
		//player.SetGoal(star);
		CallDeferred("add_child",star);
		player.CallDeferred("SetGoal",star);
		SpawnObstacles(pos);
	}
	public void SpawnObstacles(Vector2 star_pos)
	{
		GetTree().CallGroup("obstacles", "QueueFree");
		spike[] spikeArr = new spike[Math.Min(levelNum,70)];
		for (int i = 0; i < spikeArr.Length; i++)
		{
			spikeArr[i] = SpikeScene.Instantiate<spike>();
			Vector2 pos = new Vector2();
			pos.X = GD.Randf()*((float)Math.Ceiling((double)levelNum/5)*1500);
			pos.Y = -GD.Randf()*((float)Math.Ceiling((double)levelNum/5)*1500);
			spikeArr[i].Position = pos;
			CallDeferred("add_child",spikeArr[i]);
		}
		return;
	}

	public void GoalGot()
	{
		
		levelNum += 1;
		if (levelNum == 100){
			GetNode<HUD>("HUD").ShowMessage("Wow! You are an amazing player! Great Job!");
		}
		GetNode<HUD>("HUD").UpdateLevelNum(levelNum);
		
		SpawnStar();
		//SpawnObstacles();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public void StartGame()
	{
		player player = GetNode<player>("Player");
		GetNode<Timer>("SpawnBossTimer").Start();

		GetNode<HUD>("HUD").UpdateCharges(player.numCharges);
		GetNode<HUD>("HUD").ShowMessage("Collect the stars!\nWhat level can you reach?");
		SpawnStar(true);
	}

	public void SpawnPillar()
	{
		player player = GetNode<player>("Player");
		if (player.numCharges < 3){
			return;
		}
		player.numCharges -= 3;
		GetNode<HUD>("HUD").UpdateCharges(player.numCharges);

		pillar pillar = PillarScene.Instantiate<pillar>();
		pillar.value = 3;
		//pillar.Position = player.Position;
		pillar.Position = new Vector2(player.Position.X,player.Position.Y+400);
		pillar.GiveCharge += GiveCharge;

		AddChild(pillar);
	
	}

	public void Summon(string position,string classname)
	{

		player player = GetNode<player>("Player");
		if (player.numCharges <= 0){
			return;
		}
		player.numCharges -= 1;
		GetNode<HUD>("HUD").UpdateCharges(player.numCharges);
		Vector2 pos = new Vector2();
		if (position == "left")
		{
			pos = player.GetNode<Node2D>("SpawnPositionLeft").GlobalPosition;
		}
		else
		{
			pos = player.GetNode<Node2D>("SpawnPositionRight").GlobalPosition;
		}
		
		if (classname == "bounce_pad")
		{
			bounce_pad pad = BouncePadScene.Instantiate<bounce_pad>();
			pad.Position = pos;
			AddChild(pad);
		}
		else if (classname == "pillar")
		{
			pillar pillar = PillarScene.Instantiate<pillar>();
			pillar.Position = pos;
			pillar.GiveCharge += GiveCharge;
			AddChild(pillar);
		}

	}
	public void GiveCharge(int amount)
	{
		player player = GetNode<player>("Player");
		player.numCharges+=amount;
		GetNode<HUD>("HUD").UpdateCharges(player.numCharges);

	}
	public void OnSpawnBossTimerTimeout()
	{
		return;
		//ufo ufo = GetNode<ufo>("UFO");
		//ufo.SetTarget(GetNode<player>("Player"));
	}

	public void PlayerDied()
	{
		player player = GetNode<player>("Player");
		player.Position = GetNode<Node2D>("SpawnPos").Position;
		
	}
}
