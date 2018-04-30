using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugUtilities;

[RequireComponent(typeof(UnitController2D))]
[RequireComponent(typeof(PhysicsForces))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(JerboaAnimationManager))]

public class PlayerCharacter : MonoBehaviour {

	#region GameComponents
	protected UnitController2D 	PCUnitController2D;
	protected PhysicsForces 	PCPhysicsForces;
	protected JerboaAnimationManager JAnimManager;
	protected CharacterSounds	JerboaSounds;
	private Collider2D FeetCollider;
	public LayerMask PlatformLayer;
	public BodyCollider bodyCollider;
	public BalloonAnchor Balloon;
	public BalloonTriggerCollider BalloonEnterTrigger;
	#endregion

	#region PlayerStatus
	[SerializeField] 	private bool touchingPlatform;
	[SerializeField] 	private bool onGround;
						private bool wasOnGround;
	[SerializeField] 	private bool canJump;
						private bool canCancelJump;

	[SerializeField]	private bool preparingSpringJump;
	[SerializeField]	private bool warmUpJump;
	public float springJumpTimeBuffer = 10.0f;
	[SerializeField]	private float springJumpTimeCountdown = 0.0f;

	[SerializeField] 	private bool facingRight = true;
	#endregion

	#region debugging
	public bool debugging = false;

	FloatTimeLine lTimeLine = new FloatTimeLine ();
	FloatTimeLine rTimeLine = new FloatTimeLine ();

	FloatTimeLine springCountDown = new FloatTimeLine();
	#endregion

	void Awake(){
		PCUnitController2D = GetComponent<UnitController2D> ();
		PCPhysicsForces = GetComponent<PhysicsForces> ();
		JAnimManager = GetComponent<JerboaAnimationManager> ();
		FeetCollider = GetComponent<Collider2D> ();
		JerboaSounds = GetComponent<CharacterSounds> ();

		if (debugging) {
			lTimeLine.drawColor = Color.red;
			rTimeLine.drawColor = Color.green;
			//lTimeLine.turnOn ();
			//rTimeLine.turnOn ();

			springCountDown.drawColor = Color.yellow;
			springCountDown.turnOn ();
		}
	}

	void FixedUpdate(){

		CheckGround();

		JumpAction ();
		TriggerAction ();
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
		//CalcDistanceToGround ();
		wasOnGround = onGround;

		if (!PCPhysicsForces.applyGravity) {
			PCUnitController2D.setGravityScale (0);
		}
	}

	void JumpAction(){
		canJump = onGround;
		//warmUpJump = false;
		bool needToCancelJump = false;
		bool canSpringJump = false;

		bool standardJumpSound = true;
		bool warmUpJumpSound = false;
		bool springJumpSound = false;

		if (springJumpTimeCountdown > 0) {
			springJumpTimeCountdown -= Time.deltaTime;
			canSpringJump = true;
		} 
		if (springJumpTimeCountdown < 0) {
			springJumpTimeCountdown = 0;
		} 

		Balloon.setWarmUpJump (warmUpJump);

		if (PlayerInput.Instance.Jump.Down && canJump && PCPhysicsForces.applyGravity) {

			PCPhysicsForces.CalculateJumpForces ();
			PCUnitController2D.setGravityScale (PCPhysicsForces.getJumpInitialGravity());
			Vector2 jumpVector = PCPhysicsForces.getJumpVector ();

			//==========SPRINGJUMP==========
			if (preparingSpringJump) {
				standardJumpSound = false;
				warmUpJump = true;
				needToCancelJump = true;
				//JerboaSounds.PlayWarmUp ();
				warmUpJumpSound = true;
			}
			if (preparingSpringJump && canSpringJump) {
				standardJumpSound = false;
				warmUpJump = false;
				jumpVector = PCPhysicsForces.getSpringJumpVector ();
				springJumpTimeCountdown = 0;
				//JerboaSounds.PlaySpringJump ();
				warmUpJumpSound = false;
				springJumpSound = true;
			}
			//=============================
			PCUnitController2D.addImpulse (jumpVector);
			JAnimManager.JumpTakeOff ();
			canCancelJump = true;

			if (standardJumpSound) {
				JerboaSounds.PlayJump ();
			}
			if (warmUpJumpSound)
				JerboaSounds.PlayWarmUp ();
			if (springJumpSound)
				JerboaSounds.PlaySpringJump ();
		}

		if (PlayerInput.Instance.Jump.Up && canCancelJump) {
			needToCancelJump = true;
		}

		if (needToCancelJump) {
			PCUnitController2D.multiplyVelocityY (0.5f);
			JAnimManager.JumpCancelEarly ();
			canCancelJump = false;
		}

		if (debugging) {
			Vector2 myPos = transform.position;
			springCountDown.drawOrigin = myPos + new Vector2 (6f, 3f);
			springCountDown.updateValue (springJumpTimeCountdown);
			springCountDown.drawFloatTimeLine ();
		}
	}

	void TriggerAction(){

		float lTrigger = PlayerInput.Instance.LTrigger.Value;
		float rTrigger = PlayerInput.Instance.RTrigger.Value;

		if (lTrigger > 0 || rTrigger > 0) {
			preparingSpringJump = true;
		} else {
			preparingSpringJump = false;
		}

		JerboaSounds.setCharging (preparingSpringJump);

		Balloon.setTriggersHeld (preparingSpringJump);

		if (debugging) {
			Vector2 myPos = transform.position;

			lTimeLine.drawOrigin = myPos + new Vector2 (6f, 3f);
			rTimeLine.drawOrigin = myPos + new Vector2 (6f, 3f);

			lTimeLine.updateValue (lTrigger);
			rTimeLine.updateValue (rTrigger);
			lTimeLine.drawFloatTimeLine ();
			rTimeLine.drawFloatTimeLine ();
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

			if (warmUpJump && BalloonEnterTrigger.isTouchingBalloon()) {
				JerboaSounds.PlaySqueeze ();
			}
		}

		if (onGround && !wasOnGround) {
			if (warmUpJump) {
				warmUpJump = false;
				springJumpTimeCountdown = springJumpTimeBuffer;
			}
		}

		if (touchingPlatform) {
			JAnimManager.TouchDown ();
		}
	}




	#region ColliderEvents
	void OnCollisionEnter2D(Collision2D c){
		if (c.collider.tag == "SolidPlatform") {
			touchingPlatform = true;
			//JAnimManager.FoundLandingPos (Vector2.zero); //TESTING ONLY
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

	void CheckGround(){

		bool b = false;

		float gDistance = 1f;
		Vector2 gPoint = Vector2.zero;

		Vector2 down = Vector2.down;
		RaycastHit2D[] farResults = new RaycastHit2D[8];
		RaycastHit2D passResult = new RaycastHit2D();
		float closeDistance = 0.25f;
		float farDistance = 2f;
		ContactFilter2D filter = new ContactFilter2D ();
		filter.SetLayerMask (PlatformLayer);

		int numberOfFarResults = FeetCollider.Cast (down, filter, farResults, farDistance, true);

		if (numberOfFarResults > 0) {
			passResult = farResults[0];
			Collider2D other = farResults[0].collider;
			ColliderDistance2D dist2D = FeetCollider.Distance (other);
			gDistance = dist2D.distance;
			gPoint = farResults [0].point;

			if (gDistance < closeDistance) {
				b = true;
			}
		}

		onGround = b;
		JAnimManager.SetDistanceToGround (gDistance);
		JAnimManager.SetColliderCastHit(numberOfFarResults, passResult);
	}

	#endregion


}
