using UnityEngine;
using System.Collections;

public class Floater : MonoBehaviour {

	public Vector3 direction = Vector3.up;
	public float magnitude;

	private Rigidbody theRb;

	void Start () {
		theRb = gameObject.GetComponent<Rigidbody> ();

		if (theRb == null)
			Debug.LogError ("Floater error: No Rigidbody attached!");
	}

	void Update () {
		theRb.AddForce (direction * magnitude);
	}
}
