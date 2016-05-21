using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupDropper : MonoBehaviour {

	[Range (1, 100)]
	public int chanceToDrop = 1;					// 1 to 100 to drop pickup

	//this is not the most modular way to do this. I'll need to figure out a better version to 
	//use this script in other projects.

	public GameObject healthPrefab;
	public GameObject shieldPrefab;
	public GameObject scorePrefab;
	public GameObject bombPrefab;
	public GameObject lifePrefab;

	public int healthChance;
	public int shieldChance;
	public int scoreChance;
	public int bombChance;
	public int lifeChance;

	private GameObject[] prefabsArray = new GameObject[5];
	private int[] chancesArray = new int[5];
	private int vsum = 0;								// sum of pickup chances

	void Start () {
		prefabsArray [0] = healthPrefab;
		prefabsArray [1] = shieldPrefab;
		prefabsArray [2] = scorePrefab;
		prefabsArray [3] = bombPrefab;
		prefabsArray [4] = lifePrefab;

		UpdatePickupChancesSize();
		CalculateVSum ();
	}

	void Update () {
		UpdatePickupChancesSize ();
		CalculateVSum ();
	}

	// this is some really bad coding, but I'm going to skip this step for now.
	void ValidatePrefabs(){
	}
		
	void UpdatePickupChancesSize(){
		chancesArray [0] = healthChance;
		chancesArray [1] = shieldChance;
		chancesArray [2] = scoreChance;
		chancesArray [3] = bombChance;
		chancesArray [4] = lifeChance;
	}

	void CalculateVSum(){
		vsum = 0;
		for (int i = 0; i < chancesArray.Length; i++)
			vsum += chancesArray [i];
	}

	public void SpawnPickup(){
		//roll for chance
		int diceRoll = (int)Random.Range(1.0f, 100.0f);

		if (diceRoll <= chanceToDrop) {
			int pickupDropped = (int)Random.Range (1.0f, vsum);
			bool pickupFound = false;

			for (int i = 0; i < chancesArray.Length; i++) {
				pickupDropped -= chancesArray [i];

				if (pickupDropped <= 0) {
					// spawn the pickup
					Transform spawnLoc = gameObject.transform;
					GameObject.Instantiate (prefabsArray [i], spawnLoc.position, spawnLoc.rotation);
					pickupFound = true;
				}
				if (pickupFound)
					break;
			}
		}
	}
}
