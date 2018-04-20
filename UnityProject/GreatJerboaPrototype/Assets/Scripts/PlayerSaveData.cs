using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerSaveData {

	private int playerID;

	public string previousLevel;
	public string currentLevel;

	private List<LevelSaveData> levelData = new List <LevelSaveData>();

	public PlayerSaveData(int id){
		playerID = id;
	}

	public void BeginLevel(string l){
		currentLevel = l;
	}

	public string GetCurrentLevel(){
		return currentLevel;
	}

	public void CheckForLevelData(string lvl){

		if (levelData == null) {
			levelData = new List<LevelSaveData> ();
			Debug.Log ("Level Data = null |  New Level Data Created");
		}


		bool found = false;

		for (int i = 0; i < levelData.Count; i++) {
			//Debug.Log ("Comparing: lvl = " + lvl + "\t\tdataName = " + levelData[i].getName());
			if (lvl.Equals( levelData [i].getName ())) {
				//Level Data Exists
				found = true;
				Debug.Log ("Level Data Found : " + lvl);
				return;
			}
		}
		if (!found) {
			levelData.Add (new LevelSaveData (lvl));
			Debug.Log ("Level Data Not Found, Added New Level Data : " + lvl);
		}
	}

}