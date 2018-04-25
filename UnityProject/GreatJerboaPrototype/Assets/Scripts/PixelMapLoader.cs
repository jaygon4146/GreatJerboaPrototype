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
	private List<MapCell> groupedCells = new List<MapCell>();

	//private List<GameObject> spawnedObjects = new List<GameObject>();
	private static List<GameObject> collectedList = new List<GameObject>();

	private string mapLocation = "Art/LevelMaps/";
	private string mapDirectory = "Assets/Resources/Art/LevelMaps";

	private static List<string> mapFiles = new List<string>();

	private static CellPalette palette;

	private static string CurrentLevelPath;

	[SerializeField]	private float loadingProgress;

	[SerializeField]	private Vector2 PCSpawnPoint;
	[SerializeField]	private Vector2 PCGoalPoint;

	private int[] HorzGroupLengths;
	private int[] HorzGroupPosSums;

	private int[] VertGroupLengths;
	private int[] VertGroupPosSums;


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

		CurrentLevelPath = k_LevelPaths [(int)LevelEnum.TestLevel2];
		//TESTING BLOCKOUT ONLY
		CurrentLevelPath = DataManager.GetCurrentLevel ();
		CurrentLevelPath = CurrentLevelPath.Substring (0, CurrentLevelPath.Length - 4); //cut off end of the string


		string path = mapLocation + CurrentLevelPath;
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
		List<Vector2> singleCandidates = new List<Vector2> ();

		HorzGroupLengths = new int[(int)mapSize.y];
		HorzGroupPosSums = new int[(int)mapSize.y];

		VertGroupLengths = new int[(int)mapSize.x];
		VertGroupPosSums = new int[(int)mapSize.x];

		collectedList.Clear ();

		int xPos = 0;
		int yPos = 0;

		for (int i = 0; i < mapLength; i++) {
			//loadingProgress = i / mapLength;
			mapCells [i] = new MapCell (mapPixels [i], new Vector2 (xPos, yPos));

			#region Track Horizontal/Vertical Groups
			if (xPos == 0){									//If we are at the beginning of a new line
				if (HorzGroupLengths[yPos] > 0){						//check the group count
					CreateHorizontalGroupedBox(yPos);				
				}
			}
			if (yPos == mapSize.y){							//If we are at the end of a vertical line
				if (VertGroupLengths[xPos] > 0){				//check that group count
					CreateVerticalGroupedBox(xPos);
				}
			}

			if (mapCells[i].getType() == (int)CellIDs.Box){	//If this is a box
				HorzGroupLengths[yPos] ++;							//count the box
				HorzGroupPosSums[yPos] += xPos;
						
				VertGroupLengths[xPos] ++;
				VertGroupPosSums[xPos] += yPos;

				if (HorzGroupLengths[yPos] == 1 && VertGroupLengths[xPos] == 1){ 
					// if this is the first, both vertically and horizontally
					singleCandidates.Add(new Vector2(xPos, yPos));
				}
				else{
					for (int j = 0; j < singleCandidates.Count; j ++){
						bool disproved = false;
						Vector2 v = singleCandidates[j];
						if (v.x == xPos -1 && v.y == yPos)
							disproved = true;	//if there is a single candidate to my left, it is proven not single
						if (v.x == xPos && v.y == yPos -1)
							disproved = true;	//if there is a single candidate below me, it can't be single
						if (disproved){
							singleCandidates.RemoveAt(j);
							j--;
						}
						print("disproved: " + disproved);
					}
				}


			} else {										//If this is NOT a box
				SpawnCell(mapCells[i]);							//spawn this
				if (HorzGroupLengths[yPos] > 0){				//check the group count
					if (HorzGroupLengths[yPos] == 1){
						HorzGroupLengths[yPos] = 0;
						HorzGroupPosSums[yPos] = 0;						
					}else{
						CreateHorizontalGroupedBox(yPos);				
					}
				}
				if (VertGroupLengths[xPos] >0){					//check the vert group count
					if (VertGroupLengths[xPos] == 1){
						VertGroupLengths [xPos] = 0;
						VertGroupPosSums [xPos] = 0;
					}else{
						CreateVerticalGroupedBox(xPos);	
					}
				}
			}

			#endregion

			xPos++;
			if (xPos >= mapSize.x) {
				xPos = 0;
				yPos ++;
			}
		}
		print ("NumberOfSinglesFound: " + singleCandidates.Count);
		Debug.Log ("Load Map() Complete");
	}
	void CreateHorizontalGroupedBox(int y){
		Vector2 v = new Vector2 (0, y);
		MapCell cell = new MapCell (palette.getBoxColor (), v);
		float midX = (float)HorzGroupPosSums[y] / (float)HorzGroupLengths[y];
		cell.setXPos (midX);
		cell.setXScale (HorzGroupLengths[y]);
		groupedCells.Add (cell);
		//print ("New Horizontal Group Added;\tLength: " + HorzGroupLengths[y] + "\tmidX: " + midX + "\tyPos: " + y);
		int last = groupedCells.Count - 1;
		SpawnCell (groupedCells[last]);
		HorzGroupLengths[y] = 0;
		HorzGroupPosSums[y] = 0;
	}

	void CreateVerticalGroupedBox(int x){
		Vector2 v = new Vector2 (x, 0);
		MapCell cell = new MapCell (palette.getBoxColor (), v);
		float midY = (float)VertGroupPosSums [x] / (float)VertGroupLengths [x];
		cell.setYPos (midY);
		cell.setYScale (VertGroupLengths [x]);
		groupedCells.Add (cell);

		//print ("New Vertical Group Added;\tLength: " + VertGroupLengths[x] + "\tmidY: " + midY + "\txPos: " + x);
		int last = groupedCells.Count - 1;
		SpawnCell (groupedCells[last]);
		VertGroupLengths [x] = 0;
		VertGroupPosSums [x] = 0;
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

		//print ("SpawnCellPrefab()");

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

		if (cell.getType () == (int)CellIDs.Box) {
			obj.transform.localScale = cell.getScale ();
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
