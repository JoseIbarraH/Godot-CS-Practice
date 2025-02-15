using Godot;
using System;

public partial class Menu : Control
{

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
    }

    public void _on_play_button_pressed()
    {
        GetTree().ChangeSceneToFile("res://src/scenes/Main.tscn");
    }

    public void _on_exit_button_pressed()
    {
        GetTree().Quit();
    }

}
