using UnityEngine;
using System.Collections;

public class TransformBobber : MonoBehaviour {
	
	public float sineMagnitude;
	public float sineFrequency;

	void FixedUpdate () {
		float calcYPos = (Mathf.Sin ((Time.timeSinceLevelLoad) * sineFrequency) * sineMagnitude) * Time.deltaTime;
		gameObject.transform.position = new Vector3 (gameObject.transform.position.x,
			gameObject.transform.position.y + calcYPos,
			gameObject.transform.position.z);
	}
}
