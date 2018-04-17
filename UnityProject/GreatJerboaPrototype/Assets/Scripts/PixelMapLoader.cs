using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapCellSpace;

public class PixelMapLoader : MonoBehaviour {

	private Texture2D imageTexture;
	private Vector2 mapSize = new Vector2 (50,36);
	private Color[] mapPixels;
	private MapCell[] mapCells;

	private List<GameObject> spawnedObjects = new List<GameObject>();

	private string mapLocation = "Art/LevelMaps/";

	private static CellPalette palette;

	[SerializeField]	private float loadingProgress;

	#region Definitions
	public enum LevelList
	{
		TestLevel,
	}

	protected static readonly Dictionary <int, string> k_LevelPaths= new Dictionary<int, string>{
		{(int)LevelList.TestLevel, "TestLevel"},
	};
	#endregion

	void Awake () {

		palette = GetComponent<CellPalette> ();

		string path = mapLocation + k_LevelPaths[(int)LevelList.TestLevel];
		//print ("path = "+ path );

		//imageTexture = Resources.Load (path) as Texture2D;
		imageTexture = new Texture2D((int) mapSize.x, (int) mapSize.y);
		imageTexture = (Texture2D) Resources.Load(path, typeof(Texture2D));

		mapPixels = new Color[(int)mapSize.x * (int)mapSize.y];
		mapPixels = imageTexture.GetPixels ();

		LoadMap ();

		//print ("imageTexture.GetPixels();");
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
		case ((int)CellTypes.MISSING):
			Debug.Log ("SpawnCell() : MISSING");
			break;

		case ((int)CellTypes.Nothing):
			Debug.Log ("SpawnCell() : Nothing");
			break;

		case ((int)CellTypes.Box):
			Debug.Log ("SpawnCell() : Box");

			GameObject original = palette.lookUpPrefab (cell.getType());
			GameObject obj = Instantiate (
				original, 								//original
				cell.getPosition(), 					//position
				Quaternion.identity, 					//rotation
				transform								//parent
				
			);


			break;

		case ((int)CellTypes.PCSpawn):
			Debug.Log ("SpawnCell() : PCSpawn : @ :" + cell.getPosition ());

			break;

		case ((int)CellTypes.PCGoal):
			Debug.Log ("SpawnCell() : PCGoal : @ :" + cell.getPosition());
			break;	

		default:
			Debug.Log ("SpawnCell()");
			break;
		}

	}
}
