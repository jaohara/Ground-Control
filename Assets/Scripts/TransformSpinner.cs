using UnityEngine;
using System.Collections;

public class TransformSpinner : MonoBehaviour {

	public float speed = 1.0f;
	public Vector3 spinDirection = new Vector3(0.0f, 1.0f, 0.0f);

	void FixedUpdate () {
		gameObject.transform.Rotate (spinDirection * speed);
	}
}
