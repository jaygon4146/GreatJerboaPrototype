using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public PixelMapLoader MapLoader;

	void Awake(){
		MapLoader.Activate ();
		MapLoader.LoadMap();
		SpawnPlayerCharacter ();
	}


	void SpawnPlayerCharacter(){

		Vector3 PCSpawn = MapLoader.getPCSpawn ();

	}
}
