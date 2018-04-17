using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapCellSpace {

	public enum CellTypes{
		MISSING,
		Nothing,
		Box,
		PCSpawn,
		PCGoal,
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
				myType = (int)CellTypes.MISSING;
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
