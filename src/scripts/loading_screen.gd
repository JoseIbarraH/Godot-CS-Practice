extends Control

@onready var progress : Array
@onready var scene_load_status : int


# Called when the node enters the scene tree for the first time.
func _ready():
	ResourceLoader.load_threaded_request("res://src/scenes/main.tscn")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta) -> void:
	scene_load_status = ResourceLoader.load_threaded_get_status("res://src/scenes/main.tscn", progress)

	%ProgressBar.value = progress[0] * 100

func _on_progress_bar_value_changed(value):
	if value == 100 and scene_load_status == ResourceLoader.THREAD_LOAD_LOADED:
		get_tree().call_deferred("change_scene_to_packed", ResourceLoader.load_threaded_get("res://src/scenes/main.tscn"))
