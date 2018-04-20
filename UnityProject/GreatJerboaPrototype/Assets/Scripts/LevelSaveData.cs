using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelSaveData {

	private string Name = "";
	private int TimesPlayed = 0;
	private int numCollectables = 0;
	private int numCollected = 0;
	private float CompletionPercentage = 0f;
	private float RecordPercentage = 0f;

	private float startTime = 0f;
	private float finishTime = 0f;
	private float recordTime = -1f;


	private bool GoalReached = false;
	private bool Completed100 = false;

	private string LevelStatusMessage = "Level Never Played";

	public LevelSaveData(string name){
		Name = name;
	}

	public void RecordLevelData(int collected){
		GoalReached = true;
		numCollected = collected;
		TimesPlayed++;
		CalculateCompletionPercentage ();
		KeepTime ();
	}

	private void CalculateCompletionPercentage(){
		CompletionPercentage = Mathf.Round(((float)numCollected / (float)numCollectables) * 100f);

		finishTime = Time.time - startTime;

		LevelStatusMessage = ("Best Run Score: " + RecordPercentage + "%");
		LevelStatusMessage += ("\nBest Run Time: " + recordTime);
		LevelStatusMessage +=  ("\n\nYou Collected: " + numCollected + " / " + numCollectables);
		LevelStatusMessage +=  ("\t=\t" + CompletionPercentage + "%");
		LevelStatusMessage += ("\nYour Time: " + finishTime);

		if (CompletionPercentage > RecordPercentage) {
			LevelStatusMessage += ("\nYou Beat Your Top Score!");
			RecordPercentage = CompletionPercentage;
			recordTime = finishTime;
			LevelStatusMessage += ("\nNew Time Recorded");
		}

		if (RecordPercentage == 100f) {
			Completed100 = true;
		}
	}

	private void KeepTime(){

		if (CompletionPercentage == RecordPercentage) {
			if (finishTime < recordTime || recordTime <= 0) {
				recordTime = finishTime;
				LevelStatusMessage += ("\nYou Beat The Record Time!");
			}
		}	
	}

	public string getName(){
		return Name;
	}

	public string getCompletionMessage(){
		return LevelStatusMessage;
	}

	public bool isFirstPlay(){
		bool b = false;
		if (TimesPlayed == 0)
			b = true;

		return b;
	}

	public void PrepareLevelData(int numberOfCollectables){
		numCollectables = numberOfCollectables;
		startTime = Time.time;
		CalculateCompletionPercentage ();
	}


}