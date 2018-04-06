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
			if (debugging) {
				drawFloatTimeLine ();
			}
		}

		public void turnOn(){
			debugging = true;
		}

		public void turnOff(){
			debugging = false;
		}

		private void drawFloatTimeLine(){
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


	[Serializable]
	public class VisibleVector2{
		public Vector2 start;
		public Vector2 end;

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
			if (debugging)
				drawDebugLine ();
		}

		private void drawDebugLine(){
			Debug.DrawLine (start, end, drawColor);
		}

	}
}
