using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DynamicBackground : MonoBehaviour {

	public GameObject target;

	public GameObject closeLayer;
	public GameObject mediumLayer;
	public GameObject farLayer;
	public GameObject horizonLayer;

	[Range(0,1)] public float mediumLerp = 0.66f;
	[Range(0,1)] public float farLerp = 0.33f;


	private Vector2 levelCenter = new Vector2 (25f, 18f);

	//private string maskDirectory = "Assets/Resources/Art/Sprites/EnvironmentSprites/Masks";
	private string maskDirectory = "Art/Sprites/EnvironmentSprites/Masks";
	private Sprite[] maskSprites;


	void Awake(){
		RandomizeMasks ();
	}

	void RandomizeMasks(){
		maskSprites = Resources.LoadAll <Sprite>(maskDirectory);
		foreach (Sprite s in maskSprites) {
			//print (s.name);
		}

		AssignMask (closeLayer);
		AssignMask (mediumLayer);
		AssignMask (farLayer);

	}


	void AssignMask(GameObject go){
		SpriteMask m = go.GetComponent<SpriteMask> ();

		int n = Random.Range(0, maskSprites.Length);

		m.sprite = maskSprites [n];

	}

	void FixedUpdate(){
		Vector2 pos = target.transform.position;

		closeLayer.transform.position = Vector2.Lerp (pos, levelCenter, 1f);
		mediumLayer.transform.position = Vector2.Lerp (pos, levelCenter, mediumLerp);
		farLayer.transform.position = Vector2.Lerp (pos, levelCenter, farLerp);

		float yOffset = Mathf.Lerp (pos.y, levelCenter.y, 0.75f);
		//horizonLayer.transform.position = Vector2.Lerp (pos, levelCenter, 0f);
		horizonLayer.transform.position = new Vector2 (pos.x, yOffset);

	}

}
