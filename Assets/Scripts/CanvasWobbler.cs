using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasWobbler : MonoBehaviour {

	/*
	 * A stupid little effect to have a canvas zoom in and out with a sine wave. 
	 */

	public float amplitude = 1.0f;
	public float frequency = 1.0f;

	private RectTransform canvasRect;
	private float baseZOffset;
	private float currZOffset;

	void Start(){
		canvasRect = gameObject.GetComponent<RectTransform> ();
		baseZOffset = canvasRect.position.z;
	}

	void Update(){
		currZOffset = Mathf.Sin (Time.timeSinceLevelLoad * frequency) * amplitude;

		canvasRect.position = new Vector3 (canvasRect.position.x, canvasRect.position.y, baseZOffset) + new Vector3 (0, 0, currZOffset);
	}
}
