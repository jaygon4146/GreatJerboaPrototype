using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelSaveData {

	private string Name = "";
	private int numCollectables;
	private int numCollected;
	private float CompletionPercentage = 0f;

	private bool GoalReached = false;
	private bool Completed100 = false;


	public LevelSaveData(string name){
		Name = name;
	}

	public void RecordLevelData(int collected){
		GoalReached = true;
		numCollected = collected;
		CalculateCompletionPercentage ();
	}

	private void CalculateCompletionPercentage(){
		CompletionPercentage = (numCollectables / numCollected) * 100f;
		if (CompletionPercentage == 100f) {
			Completed100 = true;
		}
	}
	public string getName(){
		return Name;
	}
}