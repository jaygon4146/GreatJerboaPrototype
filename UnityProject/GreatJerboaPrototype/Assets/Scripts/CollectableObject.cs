using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour {

	[SerializeField] private bool touchingPlayerCharacter = false;
	[SerializeField] private bool HasBeenTriggered = false;



	void OnTriggerEnter2D(Collider2D c){
		touchingPlayerCharacter = true;
		HasBeenTriggered = true;
		//print ("CollectableEnter");
	}

	void OnTriggerStay2D(Collider2D c){
		touchingPlayerCharacter = true;
		//print ("CollectableStay");
	}

	void OnTriggerExit2D(Collider2D c){
		touchingPlayerCharacter = false;
		//print ("CollectableExit");
	}
}
