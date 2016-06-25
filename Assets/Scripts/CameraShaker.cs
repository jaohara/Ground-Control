using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

	public float shakeIntensity;
	public float shakeDecay;
	public float shakeSpeed;

	private Vector3 originalPosition;
	private Quaternion originalRotation;
	private Transform cameraTrans;

	void Start(){
		cameraTrans = gameObject.GetComponent<Transform> ();
	}

	public void Shake(){
		originalPosition = cameraTrans.position;
		originalRotation = cameraTrans.rotation;
		StartCoroutine ("ShakeAction");
	}

	IEnumerator ShakeAction(){
		float currentShakeIntensity = shakeIntensity;
		while (currentShakeIntensity > 0) {
			float newX = originalPosition.x + Random.Range ((-1) * currentShakeIntensity, currentShakeIntensity);
			float newY = originalPosition.y + Random.Range ((-1) * currentShakeIntensity, currentShakeIntensity);
			float newZ = originalPosition.z + Random.Range ((-1) * currentShakeIntensity, currentShakeIntensity);

			Vector3 newCompositeVector = new Vector3(newX, newY, newZ);

			cameraTrans.position = newCompositeVector;
			//cameraTrans.rotation = Quaternion.Euler(newCompositeVector);

			currentShakeIntensity -= shakeDecay;
			yield return new WaitForSeconds (1 / shakeSpeed);
		}
		cameraTrans.position = originalPosition;
		//cameraTrans.rotation = originalRotation;
	}
}
