using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public PixelMapLoader MapLoader;
	public PlayerCharacter Character;

	void Awake(){
		MapLoader.Activate ();

		Character.transform.position = MapLoader.getPCSpawnPoint () + Vector2.up *0.05f;
	}
}
