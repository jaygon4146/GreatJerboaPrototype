using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public PixelMapLoader MapLoader;
	public PlayerCharacter Character;

	public GoalTrigger Goal;

	private List<CollectableObject> collectedList = new List<CollectableObject>();

	[SerializeField] private bool LevelComplete = false;

	void Awake(){
		MapLoader.Activate ();
		Character.transform.position = MapLoader.getPCSpawnPoint () + Vector2.up *0.05f;
		Goal.transform.position = MapLoader.getPCGoalPoint ();
	}

	void FixedUpdate(){
		if (Goal.HasBeenReached () && !LevelComplete) {
			LevelComplete = true;
			print ("!!! LEVEL COMPLETED !!!");
		}
	}
}
