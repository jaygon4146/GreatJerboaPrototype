using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MapCellSpace;

public class PixelMapLoader : MonoBehaviour {

	private Texture2D imageTexture;
	private Vector2 mapSize = new Vector2 (50,36);
	private Color[] mapPixels;
	private MapCell[] mapCells;

	//private List<GameObject> spawnedObjects = new List<GameObject>();
	private static List<GameObject> collectedList = new List<GameObject>();

	private string mapLocation = "Art/LevelMaps/";
	private string mapDirectory = "Assets/Resources/Art/LevelMaps";

	private static List<string> mapFiles = new List<string>();

	private static CellPalette palette;

	[SerializeField]	private float loadingProgress;

	[SerializeField]	private Vector2 PCSpawnPoint;
	[SerializeField]	private Vector2 PCGoalPoint;

	#region Definitions
	public enum LevelEnum
	{
		TestLevel,
		TestLevel2,
	}

	protected static readonly Dictionary <int, string> k_LevelPaths= new Dictionary<int, string>{
		{(int)LevelEnum.TestLevel, "TestLevel"},
		{(int)LevelEnum.TestLevel2, "TestLevel2"},
	};
	#endregion

	void Awake () {
	}


	public void Activate(){

		palette = GetComponent<CellPalette> ();

		//string path = mapLocation + k_LevelPaths[(int)LevelList.TestLevel];
		string path = mapLocation + k_LevelPaths[(int)LevelEnum.TestLevel2];
		//print ("path = "+ path );

		//imageTexture = Resources.Load (path) as Texture2D;
		imageTexture = new Texture2D((int) mapSize.x, (int) mapSize.y);
		imageTexture = (Texture2D) Resources.Load(path, typeof(Texture2D));

		mapPixels = new Color[(int)mapSize.x * (int)mapSize.y];
		mapPixels = imageTexture.GetPixels ();

		LoadMap ();

	}

	void LoadMap(){
		int mapLength = mapPixels.Length;
		mapCells = new MapCell[mapLength];

		int xPos = 0;
		int yPos = 0;

		for (int i = 0; i < mapLength; i++) {
			loadingProgress = i / mapLength;

			mapCells [i] = new MapCell (mapPixels [i], new Vector2 (xPos, yPos));
			//mapCells [i].SpawnCell ();
			SpawnCell(mapCells[i]);

			xPos++;
			if (xPos >= mapSize.x) {
				xPos = 0;
				yPos ++;
			}
		}

		print ("LoadMap(): Complete");
	}

	void SpawnCell(MapCell cell){

		switch (cell.getType()) {

		case ((int)CellIDs.MISSING):
			//Debug.Log ("SpawnCell() : MISSING");
			break;

		case ((int)CellIDs.Nothing):
			//Debug.Log ("SpawnCell() : Nothing");
			break;

		case ((int)CellIDs.Box):
			//Debug.Log ("SpawnCell() : Box");
			SpawnCellPrefab(cell);
			break;

		case ((int)CellIDs.Collectable ):
			//Debug.Log ("SpawnCell() : Collectable : @ :" + cell.getPosition ());
			SpawnCellPrefab (cell);
			break;

		case ((int)CellIDs.PCSpawn):
			//Debug.Log ("SpawnCell() : PCSpawn : @ :" + cell.getPosition ());
			PCSpawnPoint = cell.getPosition ();
			break;

		case ((int)CellIDs.PCGoal ):
			//Debug.Log ("SpawnCell() : PCGoal : @ :" + cell.getPosition ());
			PCGoalPoint = cell.getPosition ();
			break;	

		default:
			Debug.Log ("SpawnCell() default case");
			break;
		}
	}

	private void SpawnCellPrefab(MapCell cell){

		GameObject original = palette.lookUpPrefab (cell.getType ());
		GameObject obj = Instantiate (
			original, 								//original
			cell.getPosition (), 					//position
			Quaternion.identity, 					//rotation
			transform								//parent				
		);
		string n = palette.lookUpName (cell.getType());
		obj.name = n + " @ " + cell.getPosition ();

		if (cell.getType () == (int)CellIDs.Collectable) {
			collectedList.Add (obj);
		}
	}

	public List<GameObject> getCollectableList(){
		return collectedList;
	}

	public Vector2 getPCSpawnPoint(){
		return PCSpawnPoint;
	}

	public Vector2 getPCGoalPoint(){
		return PCGoalPoint;
	}
}
