using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapCellSpace;

public class CellPalette : MonoBehaviour {

	private static Vector3 v_Missing 			= new Vector3 (-1f, -1f, -1f);
	private static Vector3 v_Nothing 			= new Vector3 (255f, 255f, 255f);
	private static Vector3 v_Box 				= new Vector3 (0f, 0f, 0f);
	private static Vector3 v_PCSpawn 			= new Vector3 (0f, 255f, 0f); 
	private static Vector3 v_PCGoal 			= new Vector3 (255f, 0f, 0f);
	private static Vector3 v_Collectable 		= new Vector3 (255f, 0f, 255f);

	public CellTemplate t_Missing 		= new CellTemplate ("Missing", v_Missing, 			(int)CellIDs.MISSING );
	public CellTemplate t_Nothing 		= new CellTemplate ("Nothing", v_Nothing, 			(int)CellIDs.Nothing );
	public CellTemplate t_Box 			= new CellTemplate ("Box", v_Box, 					(int)CellIDs.Box );
	public CellTemplate t_PCSpawn 		= new CellTemplate ("PCSPawn", v_PCSpawn, 			(int)CellIDs.PCSpawn );
	public CellTemplate t_PCGoal 		= new CellTemplate ("PCGoal", v_PCGoal,				(int)CellIDs.PCGoal );
	public CellTemplate t_Collectable 	= new CellTemplate ("Collectable", v_Collectable, 	(int)CellIDs.Collectable );

	private List<CellTemplate> TemplateList = new List <CellTemplate> ();

	public GameObject BoxPrefab;
	public GameObject CollectablePrefab;
	/*
	public static readonly Dictionary <Vector3, int> colorToType = new Dictionary<Vector3, int> {
		{v_Nothing, (int) CellTypes.Nothing},
		{v_Box, (int) CellTypes.Box},
		{v_PCSpawn, (int) CellTypes.PCSpawn},
		{v_PCGoal, (int) CellTypes.PCGoal},
		{v_Collectable, (int) CellTypes.Collectable},
	};
	*/

	void PopulateList(){
		//BoxStatic = BoxPrefab;
		TemplateList.Add (t_Missing);
		TemplateList.Add (t_Nothing);
		TemplateList.Add (t_Box);
		TemplateList.Add (t_PCSpawn);
		TemplateList.Add (t_PCGoal);
		TemplateList.Add (t_Collectable);
	}

	public bool ContainsColorVector(Vector3 v){

		for (int i = 0; i < TemplateList.Count; i++) {
			if (TemplateList[i].getColorVector() == v)
				return true;
		}
		return false;
	}

	public int lookUpType(Vector3 v){

		for (int i = 0; i < TemplateList.Count; i++) {
			if (TemplateList [i].getColorVector() == v)
				return TemplateList [i].getID ();
		}
		return -1;
	}

	public GameObject lookUpPrefab(int id){

		switch (id) {
		case (int)CellIDs.Box:
			return BoxPrefab;
			break;

		case (int)CellIDs.Collectable:
			return CollectablePrefab;
			break;

		default:
			break;
		}


		return new GameObject ();
	}

	public string lookUpName(int id){

		for (int i = 0; i < TemplateList.Count; i++) {
			if (TemplateList [i].getID() == id)
				return TemplateList [i].getName ();
		}
		return "lookUpName(): ID NOT FOUND";
	}

	public CellPalette(){
		PopulateList ();
	}


}
