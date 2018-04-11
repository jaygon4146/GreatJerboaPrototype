using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugUtilities;

public class FeetAnchor : AnchoredObject {
	
	private Vector2 prevPos;
	private Vector2 currentPos;
	private Vector2 prevVelocity;
	[SerializeField]
	private Vector2 calcVelocity;
	private Vector2 heading;

	private float groundSearchDistance;

	[SerializeField]
	private bool isJumping = false;
	private bool wasJumping = false;

	private bool rayCastHitCollider = false;

	public bool debugging = false;

	public LayerMask PlatformLayer;

	private Vector2 upwardStartVelocity;
	private float upwardStartSpeed;
	private Vector2 upwardStartPos;

	private Vector2 downwardStartPos;
	private float halfFallingHeight;

	[SerializeField]
	private bool retractingUp = false;
	[SerializeField]
	private bool extendingDown = false;

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
			upwardADebug.turnOn ();

			downwardADebug.drawColor = Color.white;
			downwardADebug.turnOn ();

			feetDestinationDebug.drawColor = Color.yellow;
			feetDestinationDebug.turnOn ();

			legReachDebug.drawColor = Color.cyan;
			legReachDebug.turnOn ();

			groundSearchDebug.drawColor = Color.green;
			groundSearchDebug.turnOn ();

			jumpPeakDebug.drawColor = Color.red;
			jumpPeakDebug.turnOn ();

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

		if (!isJumping) {
			//AdvancedClampPosition ();
		}

		if (!extendingDown && !retractingUp) {
			//AdvancedClampPosition();
		}

		if (calcVelocity.y > 0 && isJumping) {
			GoingUp ();
			extendingDown = false;
		}
		if (calcVelocity.y > 0 && isJumping) {
			GoingDown ();
			retractingUp = false;
		}

		DrawDebuggingValues ();
		prevPos = currentPos;
		prevVelocity = calcVelocity;
		wasJumping = isJumping;
	}

	void CalculateVelocity(){
		currentPos = anchor.transform.position;
		calcVelocity = currentPos - prevPos;

		float velocityY = calcVelocity.y;
		float prevY = prevVelocity.y * 0.6f;

		if (velocityY < prevY && velocityY > 0) {
			SetUpwardStartVelocity ();
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
		jumpPeakDebug.drawDebugLine ();


		retractUpLerp.drawOrigin = pos + new Vector2 (6f, 3f);
		retractUpLerp.drawFloatTimeLine ();

		extendDownLerp.drawOrigin = pos + new Vector2 (6f, 3f);
		extendDownLerp.drawFloatTimeLine ();
	}

	private Vector2 getRayCastPoint(){
		groundSearchDistance = chainLength * 2;

		if (debugging) {
			Vector2 pos = anchor.transform.position;
			legReachDebug.updateVectors (pos +  new Vector2 (-0.1f, 0f), pos + heading*chainLength) ;
			groundSearchDebug.updateVectors (pos, pos + heading * groundSearchDistance);
		}

		RaycastHit2D rayHit = Physics2D.Raycast (currentPos, heading, groundSearchDistance, PlatformLayer);

		if (rayHit.collider != null) {
			rayCastHitCollider = true;
			return rayHit.point;
		}
		rayCastHitCollider = false;
		return Vector2.zero;
	}

	private void GoingUp(){
		Vector2 anchorPos = anchor.transform.position;
		Vector2 point = getRayCastPoint ();
		Vector2 localToPoint = anchorPos - point;

		Vector2 destination = Vector2.zero;
		Vector2 fromPos = Vector2.zero;

		if (rayCastHitCollider) {
			if (localToPoint.magnitude > chainLength && !retractingUp) {
				//if our feet are out of reach, and not retracting up
				SetUpwardStartVelocity ();
				//begin retracting
			}
			if (localToPoint.magnitude < chainLength && !retractingUp) {
				//if our feet our within reach, and not retracting up
				fromPos = point;
				destination = point;
				//stay on the ground
			}
		}

		if (!rayCastHitCollider) {
			SetUpwardStartVelocity ();
		}

		if (retractingUp) {
			float t = Mathf.InverseLerp (0f, upwardStartSpeed, calcVelocity.y);
			retractUpLerp.updateValue (t);

			float deltaY = anchorPos.y - upwardStartPos.y;
			deltaY = Mathf.Clamp (deltaY, 0, chainLength);

			fromPos = new Vector2 (anchorPos.x, anchorPos.y - deltaY);
			float tY = t * deltaY;
			destination = new Vector2 (anchorPos.x, anchorPos.y - tY);
		}
		upwardADebug.updateVectors (anchorPos + Vector2.left * 1.5f, fromPos);
		feetDestinationDebug.updateVectors (anchorPos + (Vector2.left), destination);
		transform.position = destination;
	}

	private void GoingDown(){
		Vector2 anchorPos = anchor.transform.position;
		Vector2 point = getRayCastPoint ();
		Vector2 localToPoint = anchorPos - point;

		Vector2 destination = Vector2.zero;
		Vector2 fromPos = Vector2.zero;

		if (rayCastHitCollider) {
			if (localToPoint.magnitude <= groundSearchDistance && !extendingDown) {
				SetDownwardStartVelocity ();
			} else {
			}
		}

		if (!rayCastHitCollider) {
			fromPos = anchorPos;
			destination = anchorPos;
			AdvancedClampPosition ();
		}

		if (!extendingDown) {
			fromPos = anchorPos;
			destination = anchorPos;
			AdvancedClampPosition ();
		}

		if (extendingDown && rayCastHitCollider) {

			float fallingHeight = downwardStartPos.y - point.y;
			halfFallingHeight = fallingHeight / 2;

			float deltaY = downwardStartPos.y - anchorPos.y;

			float t = Mathf.InverseLerp (0, halfFallingHeight, deltaY);
			extendDownLerp.updateValue (t);

			fromPos = new Vector2 (anchorPos.x, downwardStartPos.y);
			float tY = t * chainLength;

			if (t >= 1) {
				//extendingDown = false;
			}

			destination = new Vector2 (anchorPos.x, anchorPos.y - tY);
			if (destination.y < point.y) {
				destination = new Vector2 (destination.x, point.y);
			}

		}
		downwardADebug.updateVectors (anchorPos + Vector2.right * 1.5f, fromPos);
		feetDestinationDebug.updateVectors (anchorPos + (Vector2.right), destination);
		transform.position = destination;
	}

	public void SetUpwardStartVelocity(){
		//if (!retractingUp) {
		retractingUp = true;
		upwardStartVelocity = calcVelocity;
		upwardStartSpeed = upwardStartVelocity.y;
		upwardStartPos = transform.position;
		//}
	}

	public void SetDownwardStartVelocity(){
		extendingDown = true;

		downwardStartPos = transform.position;
	}



	public void SetPullingToGround(bool b){
		//pullingToGround = b;
	}

	public void SetIsJumping(bool b){
		isJumping = b;
	}
}
