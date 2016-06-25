using UnityEngine;
using System.Collections;
/*
 * This script allows you to do a rubber-band zoom effect on your camera, IE for damage/bomb.
 * 
 * Sort of a carbon copy of the CameraShaker effect script, could probably be modified to be more 
 * tailored to this action (isn't there a rubber band camera method?)
 * 
 * This needs a bit of work. It isn't smooth yet, but sort of has the same exact effect as the 
 * CameraShaker in practice.
 * 
 * **********************
 * IDEA TO IMPLEMENT!!!!!
 * **********************
 * 
 * 	Make this rubber-banding action work in sequence with the chromatic Aberration part of the Lens Abberations 
 * camera component. this value goes from -2 to 2, resting at 0 by default. I'll have it rubber band between
 * -2 and 2, decreasing intensity each time, to ultimately rest at 0. Come to think of it, when I get this behavior
 * working it will be the exact same behavior I'd need to make this CameraZoomer script work.
 */
public class CameraZoomer : MonoBehaviour {

	public float zoomIntensity;
	public float zoomDecay;
	public float zoomSpeed;

	private float originalFOV;
	private Camera theCamera;

	void Start(){
		theCamera = gameObject.GetComponent<Camera> ();
	}

	public void RubberBandZoom(){
		originalFOV = theCamera.fieldOfView;
		StartCoroutine ("RubberBandZoomAction");
	}

	IEnumerator RubberBandZoomAction(){
		float currentZoomIntensity = zoomIntensity;
		int inverter = 1;
		while (currentZoomIntensity > 0) {
			float fovOffset = Random.Range (0.0f, currentZoomIntensity) * inverter;

			theCamera.fieldOfView = originalFOV + fovOffset;

			currentZoomIntensity -= zoomDecay;

			yield return new WaitForSeconds (1 / zoomSpeed);
		}
		theCamera.fieldOfView = originalFOV;
	}
}
