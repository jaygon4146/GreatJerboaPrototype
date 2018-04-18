using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public PixelMapLoader MapLoader;
	public PlayerCharacter Character;

	public GoalTrigger Goal;

	public GameObject CollectableHolder;

	private static List<GameObject> collectedList = new List<GameObject>();

	[SerializeField] private bool LevelComplete = false;

	void Awake(){
		MapLoader.Activate ();
		Character.transform.position = MapLoader.getPCSpawnPoint () + Vector2.up *0.05f;
		Goal.transform.position = MapLoader.getPCGoalPoint ();

		collectedList = MapLoader.getCollectableList ();
		for (int i = 0; i < collectedList.Count; i++) {
			collectedList [i].transform.SetParent (CollectableHolder.transform);
		}
	}

	void FixedUpdate(){
		if (Goal.HasBeenReached () && !LevelComplete) {
			LevelComplete = true;
			print ("!!! LEVEL COMPLETED !!!");
		}
	}
}
