using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelList : MonoBehaviour {

	public Transform scrollContent;
	public ListItemButton listItem;

	private string mapDirectory = "Assets/Resources/Art/LevelMaps";
	private static List<string> mapFiles = new List<string> ();
	private static List<ListItemButton> listItems = new List<ListItemButton> ();

	[SerializeField] int SelectedItem = 0;

	void Awake(){
		PopulateList ();
	}

	private void PopulateList(){
		//First, clear the list
		foreach (Transform child in scrollContent) {
			GameObject.Destroy (child.gameObject);
		}

		GetMapList ();
		//Populate the list
		for (int i = 0; i < mapFiles.Count ; i++) {

			ListItemButton obj = (ListItemButton)Instantiate (
				listItem,
				scrollContent
			);
			obj.InstantiateVariables (mapFiles [i], i);
			listItems.Add (obj);
		}
		UpdateSelectedItem ();
	}

	public void GetMapList(){

		DirectoryInfo dir = new DirectoryInfo (mapDirectory);
		FileInfo[] info = dir.GetFiles ("*.png*");
		foreach (FileInfo f in info) {
			//print ("File Detail");
			//print (f.Name);
			string sub = f.Name.Substring (f.Name.Length - 4);
			//print (sub);

			if (sub.Equals (".png")) {
				mapFiles.Add (f.Name);
			}
		}
	}

	public void CursorUp(){
		if (SelectedItem > 0) {
			SelectedItem--;
			UpdateSelectedItem ();
		}
	}

	public void CursorDown(){
		if (SelectedItem < mapFiles.Count-1) {
			SelectedItem++;
			UpdateSelectedItem ();
		}
	}

	public void UpdateSelectedItem(){
		for (int i = 0; i < listItems.Count; i++) {
			if (listItems [i].GetID () == SelectedItem) {
				listItems [i].SelectThis ();
			} else {
				listItems [i].UnselectThis ();
			}
		}
	}

	public void ClickSelectedItem(){
		for (int i = 0; i < listItems.Count; i++) {
			if (listItems [i].GetID () == SelectedItem) {
				listItems [i].ClickThis();
			}
		}		
	}

	public string GetSelectedItemName(){
		return listItems[SelectedItem].GetName ();
	}


}
