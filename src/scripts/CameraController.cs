using Godot;
using System;

public partial class CameraController : Node3D
{
    private CharacterBody3D player;

    [Export]
    public float SensitiveHorizontal = 0.01f;
    [Export]
    public float SensitiveVertical = 0.01f;

    public override void _Ready()
    {
        // Añadir verificación para el player
        var playersInGroup = GetTree().GetNodesInGroup("Player");
        if (playersInGroup == null || playersInGroup.Count == 0)
        {
            GD.PrintErr("No se encontró ningún nodo en el grupo 'Player'");
            return;
        }

        player = playersInGroup[0] as CharacterBody3D;
        if (player == null)
        {
            GD.PrintErr("El nodo encontrado no es un CharacterBody3D");
            return;
        }

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        // Verificar que player no sea null antes de usarlo
        if (player == null)
        {
            GD.PrintErr("La referencia al player es null");
            return;
        }

        GlobalPosition = player.GlobalPosition;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            float aux = Rotation.X - eventMouseMotion.Relative.Y * SensitiveVertical;
            aux = Mathf.Clamp(aux, -0.7f, 0.5f);
            Rotation = new(
                aux,
                Rotation.Y - eventMouseMotion.Relative.X * SensitiveHorizontal,
                Rotation.Z
            );
        }
    }
}