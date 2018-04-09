using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitController2D))]
[RequireComponent(typeof(PhysicsForces))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(JerboaAnimationManager))]

public class PlayerCharacter : MonoBehaviour {

	#region GameComponents
	protected UnitController2D 	PCUnitController2D;
	protected PhysicsForces 	PCPhysicsForces;
	protected JerboaAnimationManager JAnimManager;
	private Collider2D Collider;
	#endregion

	#region PlayerStatus
	[SerializeField]
	private bool touchingPlatform;
	[SerializeField]
	private bool canJump;
	private bool canCancelJump;
	[SerializeField]
	private bool facingRight = true;
	#endregion

	void Awake(){
		PCUnitController2D = GetComponent<UnitController2D> ();
		PCPhysicsForces = GetComponent<PhysicsForces> ();
		JAnimManager = GetComponent<JerboaAnimationManager> ();
		Collider = GetComponent<Collider2D> ();
	}

	void FixedUpdate(){

		JumpAction ();
		MoveAction ();
		CheckFacing ();
		HorizontalAction ();
		VerticalAction ();


		Vector2 clampV = new Vector2 (PCPhysicsForces.topSpeed, PCPhysicsForces.getJumpMaxVelocity());
		PCUnitController2D.setClampVelocity (clampV);

		Vector2 maxV = new Vector2 (PCPhysicsForces.topSpeed, PCPhysicsForces.getJumpInitialVelocity ());
		Vector2 minV = new Vector2 (-PCPhysicsForces.topSpeed, -PCPhysicsForces.getJumpMaxVelocity());
		JAnimManager.SetMinMaxVelocity (minV, maxV);
		JAnimManager.PassCurrentVelocity (PCUnitController2D.getVelocity());


		if (!PCPhysicsForces.applyGravity) {
			PCUnitController2D.setGravityScale (0);
		}
	}

	void JumpAction(){
		canJump = touchingPlatform;

		if (PlayerInput.Instance.Jump.Down && canJump && PCPhysicsForces.applyGravity) {
			//print ("PlayerCharacter.JumpAction()");
			PCPhysicsForces.CalculateJumpForces ();
			PCUnitController2D.setGravityScale (PCPhysicsForces.getJumpInitialGravity());
			Vector2 jumpVector = PCPhysicsForces.getJumpVector ();
			PCUnitController2D.addImpulse (jumpVector);
			JAnimManager.JumpTakeOff ();
			canCancelJump = true;
		}

		if (PlayerInput.Instance.Jump.Up && canCancelJump) {
			PCUnitController2D.multiplyVelocityY (0.5f);
			JAnimManager.TakeOffTransitionAirborne ();
			canCancelJump = false;
		}


	}


	void MoveAction(){
		float inputDead = 0.01f;
		bool activeInput = false;
		float horzInput = PlayerInput.Instance.Horizontal.Value;
		if (horzInput < -inputDead || horzInput > inputDead) {
			activeInput = true;
		}

		float direction = activeInput ? horzInput : 0f;
		float accel = activeInput ? PCPhysicsForces.acceleration : PCPhysicsForces.deceleration;

		PCUnitController2D.MoveTowardMaxVelocityX (direction, accel);
		//PCUnitController2D.addImpulse (force);	
		if (direction > 0)
			facingRight = true;

		if (direction < 0)
			facingRight = false;

	}

	void CheckFacing(){
		float rY = transform.rotation.y;

		float right = 0f;
		float left = 180f;

		if (facingRight && rY != 0) {
			transform.rotation = Quaternion.Euler (0f, right, 0f);
			//print ("Flip to Right");
		}

		if (!facingRight && rY != -1) {
			transform.rotation = Quaternion.Euler (0f, left, 0f);
			//print ("Flip to Left");
		}
	}

	void HorizontalAction(){


	}

	void VerticalAction(){
		float yVelocity = PCUnitController2D.getVelocity ().y;

		if (yVelocity < 0) {
			PCUnitController2D.setGravityScale (PCPhysicsForces.getFallingGravity());
			canCancelJump = false;
		}
	}

	#region ColliderEvents
	void OnCollisionEnter2D(Collision2D c){
		if (c.collider.tag == "SolidPlatform") {
			touchingPlatform = true;
			JAnimManager.FoundLandingPos (Vector2.zero); //TESTING ONLY
		}
	}

	void OnCollisionStay2D(Collision2D c){
		if (c.collider.tag == "SolidPlatform") {
			touchingPlatform = true;
		}
	}

	void OnCollisionExit2D(Collision2D c){
		if (c.collider.tag == "SolidPlatform") {
			touchingPlatform = false;
		}
	}
	#endregion


}
