using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetAnchor : AnchoredObject {

	private float xPos;
	private float yPos;

	void FixedUpdate(){
		//BaseFixedUpdate ();
		CalculateDistances();
		FollowAnchor ();
	}

	override public void FollowAnchor (){
		float tY = Mathf.Clamp (LocalFromAnchor.y, -chainLength, 0);

		yPos = tY + anchor.transform.position.y;
		xPos = anchor.transform.position.x;

		transform.position = new Vector3 (xPos, yPos, transform.position.z);
	}

}
