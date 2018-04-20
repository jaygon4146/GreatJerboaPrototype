using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItemButton : MonoBehaviour {

	private Button button;
	public Text Label;
	[SerializeField]	private bool Selected = false;
	private string Name = "";
	private int ID = -1;

	void Start(){
	}

	public void InstantiateVariables(string name, int key){
		button = GetComponent<Button> ();
		Name = name;
		ID = key;

		gameObject.name = Name;
		Label.text = Name;
	}

	public void ClickButton(){
		//print (Label.text + " was clicked");
	}

	public int GetID(){
		return ID;
	}

	public string GetName(){
		return Name;
	}

	public void SelectThis(){
		Selected = true;
		button.Select ();
	}

	public void UnselectThis(){
		Selected = false;
	}

	public void ClickThis(){
		//if (Selected)
			//button.onClick.Invoke ();
	}


}
