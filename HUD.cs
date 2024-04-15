using Godot;
using System;

public partial class HUD : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

	public void ShowMessage(string text)
	{
		var messageLabel = GetNode<Label>("Message");
		messageLabel.Text = text;
		messageLabel.Show();

		GetNode<Timer>("MessageTimer").Start();
	}
	
	async public void ShowGameOver()
	{
		ShowMessage("Game Over");

		var messageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(messageTimer,"timeout");

		var title = GetNode<Label>("Title");
		title.Text = "Doe The Dragon";
		title.Show();

		await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
		GetNode<Label>("Title").Show();
		GetNode<AnimatedSprite2D>("Splash").Show();

		GetNode<Label>("NumCharges").Hide();
	}
	public void UpdateCharges(int charges)
	{
		GetNode<Label>("NumCharges").Text = charges.ToString();
	}
	public void UpdateLevelNum(int levelNum)
	{
		GetNode<Label>("LevelNum").Text = levelNum.ToString();
	}

	public void OnStartButtonPressed()
	{
		GetNode<Button>("StartButton").Hide();
		GetNode<Label>("Title").Hide();
		GetNode<Label>("NumCharges").Show();
		GetNode<Label>("LevelNum").Show();
		GetNode<AnimatedSprite2D>("Splash").Hide();
		EmitSignal(SignalName.StartGame);
	}
	public void OnMessageTimerTimeout()
	{
		GetNode<Label>("Message").Hide();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("StartButton").GrabFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("show_controls")){
			GetNode<Label>("Instructions").Show();
		}
		else
		{
			GetNode<Label>("Instructions").Hide();
		}
	}
}
