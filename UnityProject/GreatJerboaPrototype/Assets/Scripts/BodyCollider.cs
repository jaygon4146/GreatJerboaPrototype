using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollider : MonoBehaviour {

	private Collider2D bodyCollider;

	[SerializeField] private bool touchingPlatform;

	void Awake () {
		bodyCollider = GetComponent<Collider2D> ();
	}

	public bool isTouchingPlatform(){
		return touchingPlatform;
	}

	#region ColliderEvents
	void OnCollisionEnter2D(Collision2D c){
		if (c.collider.tag == "SolidPlatform") {
			touchingPlatform = true;
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
	#endregion
}
