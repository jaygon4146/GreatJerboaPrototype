using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataManager {

	public static List<PlayerSaveData> SaveData = new List<PlayerSaveData>();
	private static int ActiveDataPos = 0;

	private const string saveDataPath = "/PlayerSave.gjt";

	public static string getFullPath(){
		return Application.persistentDataPath + saveDataPath;
	}

	public static void Save(){
		//Debug.Log ("Begin Save()");

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (getFullPath());
		bf.Serialize (file, DataManager.SaveData);
		file.Close ();

		Debug.Log ("Finish Save()");
	}

	public static void Load(){
		//Debug.Log ("Begin Load()");

		if (File.Exists (getFullPath())) {
			//Debug.Log ("File Exists");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (getFullPath(), FileMode.Open);
			DataManager.SaveData = (List<PlayerSaveData>)bf.Deserialize (file);
			file.Close ();
		} else {
			//Debug.Log ("File Does Not Exist");
			Save ();
			Load ();
		}
		Debug.Log ("Finish Load()");
	}

	public static void SelectSaveDataSlot(int i){
		if (i >= SaveData.Count) {
			SaveData.Add(new PlayerSaveData(i));
			Debug.Log ("New Save Slot Added");
		}

		ActiveDataPos = i;

		//Debug.Log ("Save Slot Selected: " + ActiveDataPos);
	}

	public static void LoadAllLevels(List<string> lvlList){
		for (int i = 0; i < lvlList.Count; i++) {
			SaveData [ActiveDataPos].CheckForLevelData (lvlList [i]);
		}
	}


	public static void BeginLevel(string level){
		SaveData [ActiveDataPos].BeginLevel (level);
	}

	public static string GetCurrentLevel(){
		return SaveData [ActiveDataPos].GetCurrentLevel ();
	}

}