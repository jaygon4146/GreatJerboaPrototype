using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugUtilities;

public class BalloonAnchor : AnchoredObject {

	public GameObject frontFootObj;
	private bool TriggersHeld;
	private bool PerformingWarmUpJump;

	private FloatTimeLine elasticityTimeLine = new FloatTimeLine();
	private VisibleVector2 visibleForce = new VisibleVector2();

	public bool debugging = false;
	private VisibleVector2 balloonToFeet = new VisibleVector2();

	private 

	void Awake(){
		BaseAwake();

		if (debugging) {
			elasticityTimeLine.drawColor = Color.yellow;
			visibleForce.drawColor = Color.green;
			balloonToFeet.drawColor = Color.white;

			visibleForce.turnOn ();
			elasticityTimeLine.turnOn ();
			balloonToFeet.turnOn ();
		}
	}

	void FixedUpdate(){
		CalculateDistances();
		FollowAnchor();

		if (TriggersHeld) {
			moveBelowFeet ();
		}

		if (TriggersHeld && PerformingWarmUpJump) {
			RaycastHit2D rayHit = Physics2D.Raycast (transform.position, Vector2.down, 3f);
			if (rayHit.collider != null) {
				//if We are performina a warmup jump, pull the ballon to the ground
				float x = transform.position.x;
				float y = rayHit.point.y;
				if (y < frontFootObj.transform.position.y) {
					y = frontFootObj.transform.position.y;
				}
				Vector2 newPos = new Vector2 (x, y);

				transform.position = newPos;
			} else {
				moveBelowFeet ();
			}
		}

		if (debugging) {
			float t = getElasticity () * 5;
			elasticityTimeLine.drawOrigin = anchor.transform.position + new Vector3 (2f, 1f);
			elasticityTimeLine.updateValue (t);

			Vector2 pos = transform.position;
			visibleForce.updateVectors (pos, (getElasticForce () * 2) + pos);

		}
	}

	override public void FollowAnchor(){
		SimpleElasticity();
	}

	public void setTriggersHeld(bool b){
		TriggersHeld = b;
	}

	public void setWarmUpJump(bool b){
		PerformingWarmUpJump = b;
	}

	private void moveBelowFeet(){
		Vector2 feetPos = frontFootObj.transform.position;
		Vector2 myPos = transform.position;
		float maxChange = 0.1f;

		Vector2 newPos = Vector2.MoveTowards (myPos, feetPos, maxChange);

		if (debugging) {
			balloonToFeet.updateVectors (anchorPos, newPos);
		}
		transform.position = newPos;
	}
}
