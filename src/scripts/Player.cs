using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float JumpVelocity = 4.5f;

	private Vector3 lerpDirection;
	private AnimationTree animationTree;
	private Vector2 newDir;
	private Node3D lookAt;

	public override void _Ready()
	{
		lookAt = GetTree().GetNodesInGroup("CameraController")[0].GetNode<Node3D>("LookAt") as Node3D;
		//lookAt = GetNode<Node3D>("/root/Main/CameraController/LookAt"); otra forma de obtener el nodo
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
			animationTree.Set("parameters/Transition/transition_request", "Jumping");
		}
		else
		{
			animationTree.Set("parameters/Transition/transition_request", "Strafe");
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed * (float)delta * 15f, 0.25f);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * Speed * (float)delta * 15f, 0.25f);

			lerpDirection = lerpDirection.Lerp(new(
				lookAt.GlobalPosition.X,
				GlobalPosition.Y,
				lookAt.GlobalPosition.Z
			), 0.5f);

			LookAt(lerpDirection);
			newDir = newDir.Lerp(new Vector2(inputDir.X, -inputDir.Y).Normalized(), 0.25f);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);

			newDir = newDir.Lerp(Vector2.Zero, 0.25f);
		}

		animationTree.Set("parameters/Strafe/blend_position", newDir);

		Velocity = velocity;
		MoveAndSlide();
	}
}
