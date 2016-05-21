using UnityEngine;
using System.Collections;

public class RandomMover : MonoBehaviour {

	[Range(0.0f, 200.0f)] public float RandomMax = 20.0f;
	private Rigidbody thisRb;

	void Start () {
		thisRb = gameObject.GetComponent<Rigidbody> ();

		if (thisRb == null)
			Debug.LogError ("Error: There is no rigidbody attached to this object");
		else 
			thisRb.AddForce (new Vector3 (
				Random.Range (0.0f, RandomMax), 
				Random.Range (0.0f, RandomMax), 
				Random.Range (0.0f, RandomMax)));
	}
}
