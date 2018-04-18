using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapCellSpace;

public class CellPalette : MonoBehaviour {

	private static Vector3 v_Nothing 			= new Vector3 (255f, 255f, 255f);
	private static Vector3 v_Box 				= new Vector3 (0f, 0f, 0f);
	private static Vector3 v_PCSpawn 			= new Vector3 (0f, 255f, 0f); 
	private static Vector3 v_PCGoal 			= new Vector3 (255f, 0f, 0f);
	private static Vector3 v_Collectable 		= new Vector3 (255f, 0f, 255f);

	private static CellTemplate t_Nothing 		= new CellTemplate ("Nothing", v_Nothing);
	private static CellTemplate t_Box 			= new CellTemplate ("Box", v_Box);
	private static CellTemplate t_PCSpawn 		= new CellTemplate ("PCSPawn", v_PCSpawn);
	private static CellTemplate t_PCGoal 		= new CellTemplate ("PCGoal", v_PCGoal);
	private static CellTemplate t_Collectable 	= new CellTemplate ("Collectable", v_Collectable);

	public GameObject BoxPrefab;
	public GameObject CollectablePrefab;

	public static readonly Dictionary <Vector3, int> colorToType = new Dictionary<Vector3, int> {
		{v_Nothing, (int) CellTypes.Nothing},
		{v_Box, (int) CellTypes.Box},
		{v_PCSpawn, (int) CellTypes.PCSpawn},
		{v_PCGoal, (int) CellTypes.PCGoal},
		{v_Collectable, (int) CellTypes.Collectable},
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

		switch (i) {
		case (int)CellTypes.Box:
			return BoxPrefab;
			break;

		case (int)CellTypes.Collectable:
			return CollectablePrefab;
			break;

		default:
			break;
		}


		return new GameObject ();
	}

	public string lookUpName(CellTypes type){

	}

	public CellPalette(){

	}


}
