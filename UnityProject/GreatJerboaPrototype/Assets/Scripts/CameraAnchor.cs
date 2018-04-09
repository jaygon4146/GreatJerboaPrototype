using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : AnchoredObject {
	
	void Awake(){
		BaseAwake();
	}

	void FixedUpdate(){
		CalculateDistances();
		FollowAnchor();
	}

	override public void FollowAnchor(){
		//SimpleClampPosition();
		SimpleElasticity ();
	}



}
