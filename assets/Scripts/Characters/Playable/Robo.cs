using Godot;
using System;

public partial class Robo : CharacterBody3D
{
	[Export]
	public int Speed {get; set;} = 14;

	[Export]
	public int FallAcceleratin {get; set;} = 75;

	[Export]
	public int JumpForce {get; set;} = 15;

	[Export]
	public float JumpHoldForce {get; set;} = 0.8f;

	[Export]
	public float MaxJumpTime {get; set; } = 0.8f;

	private float _jumpTime = 0;

	private Vector3 _targetVelocity = Vector3.Zero;


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		var direction = Vector3.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			direction.X -= 1.0f;
		}
		if(Input.IsActionPressed("move_left"))
		{
			direction.X += 1.0f;
		}
		if(Input.IsActionPressed("move_back"))
		{
			direction.Z -= 1.0f;
		}
		if(Input.IsActionPressed("move_forward"))
		{
			direction.Z += 1.0f;
		}

		if(IsOnFloor())
		{
			_jumpTime = 0f;

			if(Input.IsActionJustPressed("jump"))
			{
				_targetVelocity.Y = JumpForce;

			}
		}
		
		else if (!IsOnFloor() && Input.IsActionPressed("jump") && _jumpTime < MaxJumpTime)
		{
			_targetVelocity.Y += JumpHoldForce;
			_jumpTime += (float)delta;
		}
    
		if(direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(-direction);
		}

		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		if(!IsOnFloor())
		{
			_targetVelocity.Y -= FallAcceleratin * (float)delta;
		}

		Velocity = _targetVelocity;
		MoveAndSlide();
	}

}
