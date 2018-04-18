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

		private static CellPalette palette = new CellPalette();
		
		public MapCell(Color color, Vector2 position){
			colorVector = colorToVector (color);
			myPosition = position;

			if (palette.ContainsColorVector (colorVector)) {
				myType = palette.lookUpType(colorVector);
			} else {
				myType = -1;
			}
		}
		/*
		public void SpawnCell(){
			switch (myType) {
			case ((int)CellTypes.MISSING):
				Debug.Log ("SpawnCell() : MISSING");
				break;

			case ((int)CellTypes.Nothing):
				Debug.Log ("SpawnCell() : Nothing");
				break;

			case ((int)CellTypes.Box):
				Debug.Log ("SpawnCell() : Box");
				break;

			case ((int)CellTypes.PCSpawn):
				Debug.Log ("SpawnCell() : PCSpawn : @ :" + myPosition);

				break;

			case ((int)CellTypes.PCGoal):
				Debug.Log ("SpawnCell() : PCGoal : @ :" + myPosition);
				break;	

			default:
				Debug.Log ("SpawnCell()");
				break;
			}
		}
		*/

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

		private Vector3 colorToVector(Color c){
			float r = c.r * 255f;
			float g = c.g * 255f;
			float b = c.b * 255f;

			Vector3 v = new Vector3 (r, g, b);
			return v;
		}

	}


}
