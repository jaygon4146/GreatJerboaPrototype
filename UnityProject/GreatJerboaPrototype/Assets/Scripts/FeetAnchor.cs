using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugUtilities;

public class FeetAnchor : AnchoredObject {

	private VisibleVector2 visibleGroundRay = new VisibleVector2();
	private VisibleVector2 visibleGroundPoint = new VisibleVector2();
	private VisibleVector2 visibleFeetPos = new VisibleVector2();

	private Vector2 prevPos;
	private Vector2 currentPos;
	[SerializeField]
	private Vector2 calcVelocity;

	private float groundSearchDistance;

	public bool debugging = false;

	public LayerMask PlatformLayer;

	void Awake(){
		BaseAwake();

		visibleGroundRay.drawColor = Color.magenta;
		visibleGroundPoint.drawColor = Color.magenta;
		visibleFeetPos.drawColor = Color.yellow;

		if (debugging) {
			visibleGroundRay.turnOn ();
			visibleGroundPoint.turnOn ();
			visibleFeetPos.turnOn ();
		}
	}

	void FixedUpdate(){
		FollowAnchor();
	}

	override public void FollowAnchor(){		
		CalculateVelocity ();
		PullToGround ();
		CalculateDistances();
		AdvancedClampPosition();

		prevPos = currentPos;
	}

	void CalculateVelocity(){
		currentPos = transform.position;
		calcVelocity = currentPos - prevPos;
	}


	void PullToGround(){

		Vector2 pos = anchor.transform.position;
		Vector2 heading = Vector2.down;
		heading = calcVelocity;
		heading.Normalize ();
		heading.y -= 1f;
		heading.Normalize ();
		groundSearchDistance = chainLength * 2;


		if (debugging) {
			visibleGroundRay.updateVectors (pos, pos + heading * groundSearchDistance);
		}

		RaycastHit2D rayHit = Physics2D.Raycast (pos, heading, groundSearchDistance, PlatformLayer);
		if (rayHit.collider != null) {
			visibleGroundRay.drawColor = Color.green;
			visibleGroundPoint.updateVectors (pos + Vector2.right, rayHit.point);
			//transform.position = rayHit.point;
		}
		else{
			visibleGroundRay.drawColor = Color.red;
		}
	}

}
