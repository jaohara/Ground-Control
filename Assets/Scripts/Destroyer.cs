using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	public GameObject[] destroyArray;		//the objects that this boundary will destroy

	void OnTriggerEnter(Collider other){
		for (int i = 0; i < destroyArray.Length; i++) {
			if (destroyArray [i].CompareTag (other.tag))
				Destroy (other.gameObject);
		}
	}
}
