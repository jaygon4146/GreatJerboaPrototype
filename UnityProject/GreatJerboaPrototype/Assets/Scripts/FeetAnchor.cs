using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugUtilities;

public class FeetAnchor : AnchoredObject {
	
	//private Vector2 prevPos;
	//private Vector2 currentPos;
	private Vector2 prevVelocity;
	[SerializeField]
	private Vector2 currentVelocity;
	private Vector2 heading;

	private float groundSearchDistance;

	[SerializeField]
	private bool isJumping = false;
	private bool wasJumping = false;

	private bool rayCastHitCollider = false;

	public bool debugging = false;

	public LayerMask PlatformLayer;

	private Vector2 upwardStartVelocity;
	private Vector2 upwardStartOffset;

	private Vector2 downwardStartPos;
	private Vector2 landingWorldPos;

	//private float fallingImpactHeight;

	[SerializeField]
	private bool retractingUp = false;
	[SerializeField]
	private bool extendingDown = false;

	private bool touchingGround = false;

	#region DebugUtilities

	private VisibleVector2 upwardADebug = new VisibleVector2();
	private VisibleVector2 downwardADebug = new VisibleVector2();

	private VisibleVector2 feetDestinationDebug = new VisibleVector2();

	private VisibleVector2 legReachDebug = new VisibleVector2();
	private VisibleVector2 groundSearchDebug = new VisibleVector2();
	private VisibleVector2 jumpPeakDebug = new VisibleVector2();

	private FloatTimeLine retractUpLerp = new FloatTimeLine();
	private FloatTimeLine extendDownLerp = new FloatTimeLine();

	#endregion

	void Awake(){
		BaseAwake();

		if (debugging) {

			upwardADebug.drawColor = Color.white;
			//upwardADebug.turnOn ();

			downwardADebug.drawColor = Color.white;
			//downwardADebug.turnOn ();

			feetDestinationDebug.drawColor = Color.yellow;
			//feetDestinationDebug.turnOn ();

			legReachDebug.drawColor = Color.cyan;
			//legReachDebug.turnOn ();

			groundSearchDebug.drawColor = Color.green;
			//groundSearchDebug.turnOn ();

			jumpPeakDebug.drawColor = Color.red;
			//jumpPeakDebug.turnOn ();

			retractUpLerp.drawColor = Color.yellow;
			retractUpLerp.turnOn ();

			extendDownLerp.drawColor = Color.magenta;
			extendDownLerp.turnOn ();

		}
	}

	void FixedUpdate(){
		FollowAnchor();
	}

	override public void FollowAnchor(){		
		CalculateVelocity ();
		CalculateDistances();

		if (isJumping) {
			if (currentVelocity.y >= 0) {
				GoingUp ();
				touchingGround = false;
				extendingDown = false;
			} else {
				GoingDown ();
				retractingUp = false;
			}

		} else {
			AdvancedClampPosition();
		}

		DrawDebuggingValues ();
		//prevPos = anchorPos;
		prevVelocity = currentVelocity;
		wasJumping = isJumping;
	}

	void CalculateVelocity(){
		//currentPos = anchorPos;
		//currentVelocity = anchorPos - prevPos;

		float velocityY = currentVelocity.y;
		float prevY = prevVelocity.y * 0.6f;

		if (velocityY < prevY && velocityY > 0) {
			//If there is a dramatic drop in velocity, our upward jump must have been cancelled
			BeginRetractingUp ();
		}

		heading = Vector2.down;
		heading.Normalize ();
		if (heading.y > 0) {
			heading.y *= -1;
		}
		heading.y -= 1f;
		heading.Normalize ();
	}

	void DrawDebuggingValues(){
		Vector2 pos = anchor.transform.position;

		upwardADebug.drawDebugLine ();
		downwardADebug.drawDebugLine ();
		feetDestinationDebug.drawDebugLine ();

		legReachDebug.drawDebugLine ();
		groundSearchDebug.drawDebugLine ();
		//jumpPeakDebug.drawDebugLine ();


		retractUpLerp.drawOrigin = pos + new Vector2 (6f, 3f);
		if (!retractingUp) {
			retractUpLerp.updateValue (1f);
		}
		retractUpLerp.drawFloatTimeLine ();

		extendDownLerp.drawOrigin = pos + new Vector2 (6f, 3f);
		if (!extendingDown) {
			extendDownLerp.updateValue (0f);
		}
		extendDownLerp.drawFloatTimeLine ();
	}

	private Vector2 getRayCastPoint(){
		groundSearchDistance = chainLength * 2;

		if (debugging) {
			Vector2 pos = anchor.transform.position;
			legReachDebug.updateVectors (pos +  new Vector2 (-0.1f, 0f), pos + heading*chainLength) ;
			groundSearchDebug.updateVectors (pos, pos + heading * groundSearchDistance);
		}

		RaycastHit2D rayHit = Physics2D.Raycast (anchorPos, heading, groundSearchDistance, PlatformLayer);

		if (rayHit.collider != null) {
			rayCastHitCollider = true;
			return rayHit.point;
		}
		rayCastHitCollider = false;
		return Vector2.zero;
	}

	private void GoingUp(){
		
		Vector2 destination = myPos;
		Vector2 fromPos = myPos;

		if (retractingUp) {
			float iLerpY = Mathf.InverseLerp (0f, upwardStartVelocity.y, currentVelocity.y);
			retractUpLerp.updateValue (iLerpY);

			float retractingOffset = upwardStartOffset.y * iLerpY;
			float dY = anchorPos.y + retractingOffset;

			destination = new Vector2 (anchorPos.x, dY);
			fromPos = new Vector2 (anchorPos.x, anchorPos.y + upwardStartOffset.y);
		} 
		else {
			#region NOT retractingUp
			bool groundInReach = false;
			Vector2 groundPoint = getRayCastPoint ();
			if (rayCastHitCollider) {
				//The ground is in sight
				Vector2 vectorToPoint = anchorPos - groundPoint;
				if (vectorToPoint.y < chainLength) {
					groundInReach = true;
				}
			}
			if (groundInReach) {
				destination = groundPoint;
			} else {
				//The ground is out of reach &&/OR out of sight
				BeginRetractingUp ();
			}
			#endregion
		}
		upwardADebug.updateVectors (anchorPos + Vector2.left * 1.5f, fromPos);
		feetDestinationDebug.updateVectors (anchorPos + Vector2.left, destination);
		transform.position = destination;
	}

	private void GoingDown(){
		Vector2 destination = myPos;
		Vector2 fromPos = myPos;

		Vector2 groundPoint = getRayCastPoint ();

		if (extendingDown) {
			landingWorldPos = groundPoint;
			float startingDistToGround = downwardStartPos.y - groundPoint.y;
			float impactDistToGround = startingDistToGround * 0.5f;
			float currentDistToGround = anchorPos.y - groundPoint.y;

			float iLerpY = Mathf.InverseLerp (startingDistToGround, impactDistToGround, currentDistToGround);
			extendDownLerp.updateValue (iLerpY);

			float extendingOffset = chainLength * iLerpY;
			float dY = anchorPos.y - extendingOffset;

			if (dY < groundPoint.y) {
				//if our destination is below the ground, put it on the ground
				dY = groundPoint.y;
				touchingGround = true;
			} else {
				touchingGround = false;
			}

			destination = new Vector2 (anchorPos.x, dY);
			fromPos = new Vector2 (anchorPos.x, downwardStartPos.y);

		} 
		else {
			#region NOT extendingDown
			bool groundInSight = false;
			if (rayCastHitCollider){
				//The ground is in sight
				groundInSight = true;
			}
			if (groundInSight){
				BeginExtendingDown();
			}
			else{
				destination = anchorPos;
				touchingGround = false;
			}

			#endregion
		}
		downwardADebug.updateVectors (anchorPos + Vector2.right * 1.5f, fromPos);
		feetDestinationDebug.updateVectors (anchorPos + Vector2.right, destination);
		transform.position = destination;
	}

	public void BeginRetractingUp(){
		retractingUp = true;
		upwardStartVelocity = currentVelocity;
		upwardStartOffset = myPos - anchorPos;
		feetDestinationDebug.drawColor = Color.yellow;
		if (debugging) {
			//upwardADebug.turnOn ();
			//downwardADebug.turnOff ();
		}
	}

	public void BeginExtendingDown(){
		extendingDown = true;
		downwardStartPos = myPos;
		feetDestinationDebug.drawColor = Color.magenta;
		if (debugging) {
			//upwardADebug.turnOff();
			//downwardADebug.turnOn();
		}
	}

	public void SetIsJumping(bool b){
		isJumping = b;
	}

	public void PassCurrentVelocity(Vector2 v){
		currentVelocity = v;
	}

	public bool isExtendingDown(){
		return extendingDown;
	}

	public bool isTouchingGround(){
		return touchingGround;
	}
}
