using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugUtilities{

	[Serializable]
	public class FloatTimeLine{
		private float currentValue;
		private float[] valueArray = new float[64];
		public Color drawColor = Color.red;
		public Vector2 drawOrigin = Vector2.zero;
		private float drawSize = 5f;

		private bool debugging = false;

		public FloatTimeLine(){

		}

		public void updateValue(float v){
			currentValue = v;
		}

		public void turnOn(){
			debugging = true;
		}

		public void turnOff(){
			debugging = false;
		}

		public void drawFloatTimeLine(){
			if (debugging) {
				for (int i = valueArray.Length - 1; i > 0; i--) {
					valueArray [i] = valueArray [i - 1];
				}
				valueArray [0] = currentValue;

				Vector2 prevPos = drawOrigin;
				Vector2 thisPos = prevPos;

				for (int i = 0; i < valueArray.Length; i++) {
					float y = valueArray [i];
					thisPos.y = y + drawOrigin.y;
					thisPos.x = (-i / drawSize) + drawOrigin.x;
					Debug.DrawLine (prevPos, thisPos, drawColor);
					prevPos = thisPos;
				}
			}
		}

		public float getValue(){
			return currentValue;
		}
	}


	[Serializable]
	public class VisibleVector2{
		private Vector2 start;
		private Vector2 end;

		public Color drawColor = Color.red;

		private bool debugging = false;

		public VisibleVector2(){

		}

		public void turnOn(){
			debugging = true;
		}

		public void turnOff(){
			debugging = false;
		}

		public void updateVectors(Vector2 s, Vector2 e){
			start = s;
			end = e;
			drawDebugLine ();
		}

		public void drawDebugLine(){
			if (debugging)
			Debug.DrawLine (start, end, drawColor);
		}

		public Vector2 getStart(){
			return start;
		}
		public Vector2 getEnd(){
			return end;
		}


	}



	[Serializable]
	public class VisibleBool{
		private bool isTrue;

		public Vector2 drawOrigin = Vector2.zero;

		public Color trueColor = Color.green;
		public Color falseColor = Color.red;

		private bool debugging = false;

		public VisibleBool(){

		}

		public void turnOn(){
			debugging = true;
		}
		public void turnOff(){
			debugging = false;
		}

		public void updateValue(bool b){
			isTrue = b;
		}

		public void drawBoolLine(){
			Color drawColor = falseColor;

			if (isTrue)
				drawColor = trueColor;
			
			Debug.DrawLine (drawOrigin, drawOrigin + Vector2.down * 5, drawColor);
		}


		public bool getValue(){
			return isTrue;
		}
	}




}
