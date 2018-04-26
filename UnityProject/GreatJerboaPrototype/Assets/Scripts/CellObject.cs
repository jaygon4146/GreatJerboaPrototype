using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObject : MonoBehaviour {

	public SpriteRenderer renderer;
	public GameObject collider;

	public void PassScale(Vector2 v){
		renderer.size = v;
		Vector3 scale = new Vector3 (v.x, v.y, 1);
		collider.transform.localScale = scale;
	}

}
