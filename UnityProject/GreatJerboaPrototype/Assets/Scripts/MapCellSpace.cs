using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapCellSpace {
	
	public enum CellIDs{
		MISSING,
		Nothing,
		Box,
		PCSpawn,
		PCGoal,
		Collectable,
	}

	public class CellTemplate{
		private Vector3 ColorVector;
		private GameObject Prefab;
		private string Name;
		private int MyID;

		public CellTemplate(string name, Vector3 color, int templateId){
			Name = name;
			ColorVector = color;
			MyID = templateId;
		}

		public Vector3 getColorVector(){
			return ColorVector;
		}

		public int getID(){
			return MyID;
		}

		public string getName(){
			return Name;
		}
	}



	public class MapCell{
		private Vector3 colorVector;
		private int myType;
		private Vector2 myPosition;
		private Vector2 myScale = new Vector2 (1f, 1f);

		private bool[] neighbours = new bool[9];

		private static CellPalette palette = new CellPalette();	
		
		public MapCell(Color color, Vector2 position){
			colorVector = colorToVector (color);
			myPosition = position;

			if (palette.ContainsColorVector (colorVector)) {
				myType = palette.lookUpType(colorVector);
			} else {
				myType = -1;
			}
			setUpNeighbours ();
		}

		private void setUpNeighbours(){
			neighbours [0] = false;		neighbours [1] = false;		neighbours [2] = false;
			neighbours [3] = false;		neighbours [4] = true;		neighbours [5] = false;
			neighbours [6] = false;		neighbours [7] = false;		neighbours [8] = false;
			//Debug.Log ("setUpNeighbours()");
		}

		public void setXScale(int x){
			myScale = new Vector2 (x, myScale.y);
		}

		public void setYScale(int y){
			myScale = new Vector2 (myScale.x, y);
		}

		public void setXPos(float x){
			myPosition = new Vector2 (x, myPosition.y);
		}
		public void setYPos(float y){
			myPosition = new Vector2 (myPosition.x, y);
		}


		public int getType(){
			return myType;
		}

		public string getTypeString(){
			string s = "";
			return s;
		}

		public Vector2 getPosition(){
			return myPosition;
		}
		public Vector2 getScale(){
			return myScale;
		}

		private Vector3 colorToVector(Color c){
			float r = c.r * 255f;
			float g = c.g * 255f;
			float b = c.b * 255f;

			Vector3 v = new Vector3 (r, g, b);
			return v;
		}

	}


}
