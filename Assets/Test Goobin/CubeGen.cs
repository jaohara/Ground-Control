using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeGen : MonoBehaviour {

	List<GameObject> cubeList = new List<GameObject>();
	public Transform cubeParent;
	public PhysicMaterial cubePhysicMat;

	int cubeGenMax = 16;

	void Start () {
		for (int i = 0; i < cubeGenMax; i++) {
			for (int j = 0; j < cubeGenMax; j++) {
				cubeList.Add (GameObject.CreatePrimitive (PrimitiveType.Cube));
				GameObject currentCube = cubeList [cubeList.Count - 1];
				Transform currentCubeTrans = currentCube.transform;
				Material currentCubeMat = currentCube.GetComponent<MeshRenderer> ().material;

				currentCube.name = currentCube.name + "(" + i + ", " + j + ")";
				currentCubeTrans.parent = cubeParent;
				currentCubeTrans.position = new Vector3 (i, 0, j);
				currentCubeTrans.localScale = new Vector3(1.0f, Random.Range(0.25f, 1.0f), 1.0f);

				float randomShade = Random.Range (0.45f, 1.0f);
				currentCubeMat.color = new Color (randomShade, randomShade, randomShade);

				currentCube.AddComponent<Rigidbody> ();
				//currentCube.GetComponent<Rigidbody> ().useGravity = false;

				if (cubePhysicMat != null)
					currentCube.GetComponent<BoxCollider> ().material = cubePhysicMat;
			}
		}
	}

	void Update () {
		//Debug.Log (cubeList.ToString ());
	}
}
