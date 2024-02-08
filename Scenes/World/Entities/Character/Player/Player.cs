using Godot;
using System;
using Game.Content;
using KludgeBox;
using KludgeBox.Scheduling;

public partial class Player : Character
{
	[Export] [NotNull] public Sprite2D ShieldSprite { get; private set; }
	
	public int Xp { get; set; }

	public int Level { get; set; } = 1;

	public double PrimaryDamage { get; set; } = 1000;
	public double PrimaryDistance { get; set; } = 2000;
	
	public double SecondaryDamage { get; set; } = 5;
	public double SecondaryDistance { get; set; } = 1000;

	public Camera Camera;

	public Cooldown SecondaryCd { get; set; } = new(0.1);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		Root.Instance.EventBus.Publish(new PlayerReadyEvent(this));
		
		Camera = GetParent().GetChild<Camera>();
		SecondaryCd.Ready += AttackSecondary;

		AttackSpeed = 3;
		Died += () =>
		{
			var mainMenu = Root.Instance.PackedScenes.Main.MainMenu;
			Root.Instance.Game.MainSceneContainer.ChangeStoredNode(mainMenu.Instantiate());
			Audio2D.StopMusic();
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		Root.Instance.EventBus.Publish(new PlayerProcessEvent(this, delta));
		
		AttackPrimary(delta);

		Hp += RegenHpSpeed * delta;
		Hp = Math.Min(Hp, MaxHp);

		SecondaryCd.Update(delta);

		ShieldSprite.Modulate = Modulate with { A = (float)HitFlash };
		
		// Camera shift processing
		if (Input.IsActionPressed(Keys.CameraShift))
		{
			var maxShift = GetGlobalMousePosition() - GlobalPosition;
			var zoomFactor = (Camera.Zoom.X + Camera.Zoom.Y) / 2;
			Camera.PositionShift = maxShift * 0.7 * zoomFactor;
		}
		else
		{
			Camera.PositionShift = Vec();
		}
		
	}

	public override void _PhysicsProcess(double delta)
	{
		Root.Instance.EventBus.Publish(new PlayerPhysicsProcessEvent(this, delta));
	}

	private void AttackPrimary(double delta)
	{
		SecToNextAttack -= delta;
		if (!Input.IsActionPressed(Keys.AttackPrimary)) return;
		if (SecToNextAttack > 0) return;

		SecToNextAttack = 1.0 / AttackSpeed;
		
		// Создание снаряда
		Bullet bullet = Root.Instance.PackedScenes.World.Bullet.Instantiate() as Bullet;
		// Установка начальной позиции снаряда
		bullet.GlobalPosition = GlobalPosition;
		// Установка направления движения снаряда
		bullet.Rotation = Rotation;
		bullet.Author = Bullet.AuthorEnum.PLAYER;
		bullet.Speed *= 3;
		bullet.RemainingDamage = PrimaryDamage;
		bullet.RemainingDistance = PrimaryDistance;
		bullet.GetNode<Sprite2D>("Sprite2D").Scale *= 2;
		bullet.Source = this;
		Audio2D.PlaySoundAt(Sfx.SmallLaserShot, Position, 1f);
		GetParent().AddChild(bullet);
	}
	
	private void AttackSecondary()
	{
		if (!Input.IsActionPressed(Keys.AttackSecondary)) return;
		
		Audio2D.PlaySoundAt(Sfx.SmallLaserShot, Position, 0.5f);
		var bulletsCount = 5;
		var spread = Mathf.DegToRad(18);
		var speedSpread = 0.1;
		
		for (int i = 0; i < 5; i++)
		{
			// Создание снаряда
			Bullet bullet = Root.Instance.PackedScenes.World.Bullet.Instantiate() as Bullet;
			// Установка начальной позиции снаряда
			bullet.GlobalPosition = GlobalPosition;
			// Установка направления движения снаряда
			bullet.Rotation = Rotation + Rand.Range(-spread, spread);
			bullet.Author = Bullet.AuthorEnum.PLAYER;
			bullet.Speed = bullet.Speed * 2 + Rand.Range(-bullet.Speed * speedSpread, bullet.Speed * speedSpread);
			bullet.RemainingDistance = SecondaryDistance;
			bullet.RemainingDamage = SecondaryDamage;
			var modulate = bullet.GetNode<Sprite2D>("Sprite2D").Modulate;
			bullet.GetNode<Sprite2D>("Sprite2D").SelfModulate = modulate.Darkened(0.2f);
			bullet.Source = this;
			GetParent().AddChild(bullet);
		}
		
	}
}
