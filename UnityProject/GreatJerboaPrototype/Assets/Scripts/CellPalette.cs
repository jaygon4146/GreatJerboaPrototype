using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapCellSpace;

public class CellPalette : MonoBehaviour {

	private static Vector3 v_Nothing = new Vector3 (255f, 255f, 255f);
	private static Vector3 v_Box = new Vector3 (0f, 0f, 0f);
	private static Vector3 v_PCSpawn = new Vector3 (0f, 255f, 0f); 
	private static Vector3 v_PCGoal = new Vector3 (255f, 0f, 0f);

	public GameObject BoxPrefab;

	//private static GameObject BoxStatic;

	public static readonly Dictionary <Vector3, int> colorToType = new Dictionary<Vector3, int> {
		{v_Nothing, (int)CellTypes.Nothing},
		{v_Box, (int)CellTypes.Box},
		{v_PCSpawn, (int) CellTypes.PCSpawn},
		{v_PCGoal, (int) CellTypes.PCGoal},
	};

	public static readonly Dictionary <int, GameObject> typeToGameObject = new Dictionary<int, GameObject>{
		//{(int)CellTypes.Box, BoxStatic},
		//{2, BoxStatic},
	};

	void Awake(){
		//BoxStatic = BoxPrefab;
	}

	public bool ContainsColorVector(Vector3 v){
		return colorToType.ContainsKey (v);
	}

	public int lookUpType(Vector3 v){
		return colorToType [v];
	}

	public GameObject lookUpPrefab(int i){

		//int t = (int)CellTypes.Box;

		//GameObject r = typeToGameObject[i];
		//GameObject r = new GameObject();

		switch (i) {
		case (int)CellTypes.Box:
			return BoxPrefab;
			break;

		default:
			break;
		}


		return new GameObject();
	}

	public CellPalette(){

	}


}
