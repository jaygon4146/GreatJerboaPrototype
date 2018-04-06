using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DebugUtilities;


public class BalloonAnchor : AnchoredObject {

	private FloatTimeLine elasticityTimeLine = new FloatTimeLine();
	private VisibleVector2 visibleForce = new VisibleVector2();

	public bool debugging = false;

	void Awake(){
		BaseAwake();
		elasticityTimeLine.drawColor = Color.yellow;
		visibleForce.drawColor = Color.green;

		if (debugging) {
			visibleForce.turnOn ();
			elasticityTimeLine.turnOn ();
		}
	}

	void FixedUpdate(){
		CalculateDistances();
		FollowAnchor();

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
}
