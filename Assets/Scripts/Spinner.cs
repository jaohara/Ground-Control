using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

	public float speed;
	public Vector3 spinDirection = Vector3.down;

	private Rigidbody theRb;

	void Start () {
		theRb = gameObject.GetComponent<Rigidbody> ();

		if (theRb == null)
			Debug.LogError ("Spinner error: No attached rigidbody!");
	}

	void Update () {
		theRb.AddTorque (spinDirection * speed);
	}
}
