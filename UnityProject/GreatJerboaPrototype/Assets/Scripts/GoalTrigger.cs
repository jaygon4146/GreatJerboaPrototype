using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour {

	[SerializeField] private bool touchingPlayerCharacter = false;
	[SerializeField] private bool GoalHasBeenTriggered = false;

	private SpriteRenderer renderer;
	private AudioSource source;
	public AudioClip clip;

	int flashCount = 0;

	void Awake(){
		renderer = GetComponent<SpriteRenderer> ();
		source = GetComponent<AudioSource> ();
	}

	void Update(){
		if (GoalHasBeenTriggered) {
			if (flashCount == 0) {
				flashCount = 25;
				float r = Random.Range (0f, 1f);
				float g = Random.Range (0f, 1f);
				float b = Random.Range (0f, 1f);

				renderer.color = new Color (r, g, b);
			}
			flashCount--;
		}
	}

	public bool HasBeenReached(){
		return GoalHasBeenTriggered;
	}

	//=============================================================
	//This Collider only interacts with the Player Character layer
	//=============================================================

	void OnTriggerEnter2D(Collider2D c){
		touchingPlayerCharacter = true;
		GoalHasBeenTriggered = true;
		source.PlayOneShot (clip);
		//print ("GoalEnter");
	}

	void OnTriggerStay2D(Collider2D c){
		touchingPlayerCharacter = true;
		//print ("GoalStay");
	}

	void OnTriggerExit2D(Collider2D c){
		touchingPlayerCharacter = false;
		//print ("GoalExit");
	}		
}
