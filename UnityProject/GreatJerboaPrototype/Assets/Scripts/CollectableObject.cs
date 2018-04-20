using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour {

	[SerializeField] private bool touchingPlayerCharacter = false;
	[SerializeField] private bool HasBeenTriggered = false;

	private SpriteRenderer renderer;

	void Awake(){
		renderer = GetComponent<SpriteRenderer> ();
	}

	public bool wasCollected(){
		return HasBeenTriggered;
	}

	void OnTriggerEnter2D(Collider2D c){
		touchingPlayerCharacter = true;
		HasBeenTriggered = true;
		renderer.color = Color.blue;
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
