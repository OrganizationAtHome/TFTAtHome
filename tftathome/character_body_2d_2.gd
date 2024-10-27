extends CharacterBody2D
	 
 
func _physics_process(_delta):
	if is_multiplayer_authority():
		velocity = Input.get_vector("ui_left","ui_right","ui_up","ui_down") * 10
	move_and_slide()
