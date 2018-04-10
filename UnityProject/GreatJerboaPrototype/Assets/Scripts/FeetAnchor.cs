using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugUtilities;

public class FeetAnchor : AnchoredObject {

	private VisibleVector2 visibleGroundRay = new VisibleVector2();
	private VisibleVector2 visibleGroundPoint = new VisibleVector2();
	private VisibleVector2 visibleFeetStretch = new VisibleVector2();
	private VisibleVector2 visibleFeetPos = new VisibleVector2();
	private VisibleVector2 visibleRetractPos = new VisibleVector2();

	private FloatTimeLine groundPointTimeLine = new FloatTimeLine();
	private FloatTimeLine feetInverseLerp = new FloatTimeLine();

	private Vector2 prevPos;
	private Vector2 currentPos;
	private Vector2 prevVelocity;
	[SerializeField]
	private Vector2 calcVelocity;
	private Vector2 heading;

	private float groundSearchDistance;

	private Vector2 startVelocity;
	private float startMagnitude;


	[SerializeField]
	private bool pullingToGround = false;
	private bool wasPullingToGround = false;
	private bool rayCastHitCollider = false;

	public bool debugging = false;

	public LayerMask PlatformLayer;

	void Awake(){
		BaseAwake();

		visibleGroundRay.drawColor = Color.magenta;
		visibleGroundPoint.drawColor = Color.magenta;
		visibleFeetPos.drawColor = Color.yellow;
		visibleRetractPos.drawColor = Color.green;
		visibleFeetStretch.drawColor = Color.cyan;

		groundPointTimeLine.drawColor = Color.green;
		feetInverseLerp.drawColor = Color.yellow;

		if (debugging) {
			visibleGroundRay.turnOn ();
			visibleGroundPoint.turnOn ();
			visibleFeetPos.turnOn ();
			visibleRetractPos.turnOn ();
			visibleFeetStretch.turnOn ();
			groundPointTimeLine.turnOn ();
			feetInverseLerp.turnOn ();
		}
	}

	void FixedUpdate(){
		FollowAnchor();
	}

	override public void FollowAnchor(){		
		CalculateVelocity ();

		if (pullingToGround) {
			PullToGround ();
		} 
		else {//notPullingToGround
			if (wasPullingToGround) {
				startVelocity = calcVelocity;
				startMagnitude = startVelocity.magnitude;			
			}

			if (calcVelocity.y > 0) {
				PullToGround ();	
				PullToZero();
			} 
			else {
				PullToGround ();	
			}
		}

		CalculateDistances();
		AdvancedClampPosition();

		prevPos = currentPos;
		prevVelocity = calcVelocity;
		wasPullingToGround = pullingToGround;
	}

	void CalculateVelocity(){
		currentPos = anchor.transform.position;
		calcVelocity = currentPos - prevPos;

		float velocityMagnitude = calcVelocity.magnitude;
		float prevMagnitude = prevVelocity.magnitude * 0.6f;

		if (velocityMagnitude < prevMagnitude && pullingToGround) {	// if there is a dramatic drop in velocity, our jump must have been cancelled
			startMagnitude = calcVelocity.magnitude;
		}

		heading = Vector2.down;
		heading.Normalize ();
		if (heading.y > 0) {
			heading.y *= -1;
		}
		heading.y -= 1f;
		heading.Normalize ();
	}

	private Vector2 getRayCastPoint(){
		groundSearchDistance = chainLength * 2;
		RaycastHit2D rayHit = Physics2D.Raycast (currentPos, heading, groundSearchDistance, PlatformLayer);

		if (rayHit.collider != null) {
			rayCastHitCollider = true;
			return rayHit.point;
		}
		rayCastHitCollider = false;
		return Vector2.zero;
	}

	void PullToGround(){

		//Vector2 pos = anchor.transform.position;

		Vector2 searchVectorEnd = currentPos + heading * groundSearchDistance;
		Vector2 fullStretch = currentPos + heading * chainLength;
		float groundPointMagnitude = 0;
		float feetILerp = 0;

		Vector2 point = getRayCastPoint();
		if (rayCastHitCollider) {
			visibleGroundRay.drawColor = Color.green;
			visibleGroundPoint.updateVectors (currentPos + Vector2.right, point);

			groundPointMagnitude = (point - currentPos).magnitude;

			feetILerp = Mathf.InverseLerp (groundSearchDistance, chainLength, groundPointMagnitude);
			Vector2 fPos = currentPos + heading * chainLength * feetILerp;

			if (feetILerp >= 1) {
				fPos = point;
			}

			visibleFeetPos.updateVectors (currentPos + Vector2.right, fPos);
			transform.position = fPos;
		}
		else{
			visibleGroundRay.drawColor = Color.red;
			transform.position = anchor.transform.position;
		}

		if (debugging) {
			/*
			visibleGroundRay.updateVectors (currentPos, searchVectorEnd);
			visibleFeetStretch.updateVectors (currentPos + Vector2.right, fullStretch);
			groundPointTimeLine.drawOrigin = (currentPos + new Vector2 (3, 1));
			groundPointTimeLine.updateValue (groundPointMagnitude);
			feetInverseLerp.drawOrigin = (currentPos + new Vector2 (3, 2));
			feetInverseLerp.updateValue (feetILerp * 2);
			*/

		}
	}

	void PullToZero(){

		Vector2 searchVectorEnd = currentPos + heading * groundSearchDistance;
		Vector2 fullStretch = currentPos + heading * chainLength;

		float retractLerp = 0f;

		Vector2 point = getRayCastPoint ();
		if (rayCastHitCollider) {
			visibleGroundRay.drawColor = Color.green;

			retractLerp = Mathf.InverseLerp (startMagnitude, 0, calcVelocity.magnitude);
			//Vector2 fPos = currentPos + heading * chainLength * retractLerp;
			Vector2 fPos = Vector2.Lerp(point, anchor.transform.position, retractLerp);

			visibleRetractPos.updateVectors (currentPos + Vector2.left, fPos);
			transform.position = fPos;

		} else {
			visibleGroundRay.drawColor = Color.red;

		}

		if (debugging) {
			
			visibleGroundRay.updateVectors (currentPos, searchVectorEnd);
			visibleFeetStretch.updateVectors (currentPos + Vector2.right, fullStretch);
			groundPointTimeLine.drawOrigin = (currentPos + new Vector2 (3, 1));
			//groundPointTimeLine.updateValue (groundPointMagnitude);
			feetInverseLerp.drawOrigin = (currentPos + new Vector2 (3, 2));
			feetInverseLerp.updateValue (retractLerp * 2);
		}
	}

	public void UpdateJumpVelocity(){
		startVelocity = calcVelocity;
		startMagnitude = startVelocity.magnitude;	
	}


	public void SetPullingToGround(bool b){
		pullingToGround = b;
	}





}
