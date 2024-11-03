extends CharacterBody2D
	 
var acc: float = 20.0
var deacc: float = 20.0
var MaxSpeed: float = 400
var MaxCrouchSpeed: float = 150
@export var VelocityLine: Line2D;
@export var InputLine: Line2D;
@export var WelcomeText: RichTextLabel;
 

func _physics_process(_delta):
	VelocityLine.set_point_position(0, WelcomeText.get_screen_position())
	VelocityLine.set_point_position(1, WelcomeText.get_screen_position()*velocity)
	if is_multiplayer_authority():
		var inputVector = Input.get_vector("ui_left","ui_right","ui_up","ui_down");
		var inputLength = inputVector.length()
		var isCrouched = Input.is_key_pressed(KEY_CTRL);
		
		if (inputLength != 0):
			if (velocity.length() == 0):
				velocity = inputVector * (velocity.length() + acc)
			else:
				var actualVector = inputVector * acc;
				var relativeAngle = 180 / actualVector.dot(velocity)
				print(relativeAngle)
				print(velocity)
				velocity.x = velocity.x + (actualVector.x)
				velocity.y = velocity.y + (actualVector.y)
		elif (velocity.length() < deacc):
			velocity.x = 0
			velocity.y = 0
		else: 
			velocity = velocity * ((velocity.length() - deacc) / velocity.length())
	move_and_slide()
