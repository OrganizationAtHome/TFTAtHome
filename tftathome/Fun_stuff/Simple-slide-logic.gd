extends CharacterBody2D
	 
 
func _physics_process(_delta):
	if is_multiplayer_authority():
		velocity = Input.get_vector("ui_left","ui_right","ui_up","ui_down") * (400 - (250 * int(Input.is_key_pressed(KEY_CTRL))))
	move_and_slide()
