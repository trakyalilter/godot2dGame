using Godot;
public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	private int _score;
	private void game_over()
{	GetNode<AudioStreamPlayer>("Music").Stop();
	GetNode<AudioStreamPlayer>("DeathSound").Play();
	GetNode<HUD>("HUD").ShowGameOver();
	GetNode<Timer>("MobTimer").Stop();
	GetNode<Timer>("ScoreTimer").Stop();
	
}
public void NewGame()
{
	var hud = GetNode<HUD>("HUD");
	hud.UpdateScore(_score);
	hud.ShowMessage("Get Ready!");
	_score = 0;

	var player = GetNode<Player>("Player");
	var startPosition = GetNode<Marker2D>("StartPosition");
	player.Start(startPosition.Position);

	GetNode<Timer>("StartTimer").Start();
	GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
	GetNode<AudioStreamPlayer>("Music").Play();
}
private void _on_mob_timer_timeout()
{
	
	// Create a new instance of the Mob scene.
	Mob mob = MobScene.Instantiate<Mob>();

	// Choose a random location on Path2D.
	var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
	mobSpawnLocation.ProgressRatio = GD.Randf();

	// Set the mob's direction perpendicular to the path direction.
	float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

	// Set the mob's position to a random location.
	mob.Position = mobSpawnLocation.Position;

	// Add some randomness to the direction.
	direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
	mob.Rotation = direction;

	// Choose the velocity.
	var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
	mob.LinearVelocity = velocity.Rotated(direction);

	// Spawn the mob by adding it to the Main scene.
	AddChild(mob);
}
private void _on_score_timer_timeout()
{
	  _score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
}
private void _on_start_timer_timeout()
{
	GetNode<Timer>("MobTimer").Start();
	GetNode<Timer>("ScoreTimer").Start();
}
}
