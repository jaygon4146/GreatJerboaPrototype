using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonTriggerCollider : MonoBehaviour {

	//Collider2D myTrigger;
	[SerializeField]	private bool touchingBalloon;

	void Awake(){
		//myTrigger = GetComponent<Collider2D> ();
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.tag == "Balloon") {
			//print ("BalloonTriggerEnter");
			touchingBalloon = true;
		}

	}
	void OnTriggerStay2D(Collider2D c){
		if (c.tag == "Balloon") {
			//print ("BalloonTriggerStay");
			touchingBalloon = true;
		}

	}
	void OnTriggerExit2D(Collider2D c){
		if (c.tag == "Balloon") {
			//print ("BalloonTriggerExit");
			touchingBalloon = false;
		}
	}

	public bool isTouchingBalloon(){
		return touchingBalloon;
	}
}
