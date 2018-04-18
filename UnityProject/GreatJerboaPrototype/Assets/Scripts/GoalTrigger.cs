using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour {

	[SerializeField] private bool touchingPlayerCharacter = false;
	[SerializeField] private bool GoalHasBeenTriggered = false;

	public bool HasBeenReached(){
		return GoalHasBeenTriggered;
	}



	//=============================================================
	//This Collider only interacts with the Player Character layer
	//=============================================================

	void OnTriggerEnter2D(Collider2D c){
		touchingPlayerCharacter = true;
		GoalHasBeenTriggered = true;
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
