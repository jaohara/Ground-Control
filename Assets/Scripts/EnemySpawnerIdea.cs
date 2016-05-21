using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnerIdea : MonoBehaviour
{
	public GameObject markerPrefab;				// what do we spawn to mark the nav points?
	public GameObject enemyPrefab;				// what do we spawn to move along the nav points?
	public float markerLifetime;
	public float spread;

	// these are more test variables
	public float enemyCount;
	public float enemySpeed;
	public float enemyMoveDelay;
	public float spawnDelay;

	public float sequentialXOffset;
	public float sequentialYOffset;

	private const int leftClearX = -25;			// x pos of the left bound to nav to for destruction
	private const int rightClearX = 25;			// x pos of the right bound to nav to for destruction

	private bool spawnEnemy = false;
	private bool spawnMultiple = false;

	private bool spawnReset = false;

	public bool canSpawn = false;

	private Transform playerTarget;
	private Vector3 playerLocation;
	private Vector3 enemySpawnPoint;

	void Start (){
		ReferencePlayerTarget ();
	}
	
	// Update is called once per frame
	void Update (){
		// canspawn wrapper is for disable/enable controls
		if (canSpawn) {
			ReferencePlayerTarget ();

			//SpawnCheck ();
			if (!spawnReset)
				TempSpawnRandom ();
			
			if (playerTarget != null) {
				if (spawnEnemy) {
					//EnemySpawnSide side = RandomSideLR ();
					//CalculateSpawnPos (side);
					//GrabPlayerLocation ();
					//SpawnEnemy (enemySpawnPoint);
					StartCoroutine ("SpawnMultiple", 1);
					spawnEnemy = false;
					Invoke ("ResetSpawnReset", Random.Range (1.0f, 4.0f));
				}
				if (spawnMultiple) {
					StartCoroutine ("SpawnMultiple", enemyCount);
					spawnMultiple = false;
					Invoke ("ResetSpawnReset", Random.Range (1.0f, 4.0f));
				}
			}
		}
	}

	// if I plan to keep this I should move this into another class, a debug class that input talks to and forwards to 
	// controllers like this.
	void SpawnCheck(){
		if (Input.GetKeyDown ("i"))
			spawnEnemy = true;

		if (Input.GetKeyDown ("o"))
			spawnMultiple = true;

		//bind the dpad axes to spawn triggers
	}

	EnemySpawnSide RandomSideLR(){
		EnemySpawnSide side;
		int roll = Random.Range (0, 200);
		if (roll % 2 == 0) {
			return EnemySpawnSide.Right;
		} else {
			return EnemySpawnSide.Left;
		}
	}

	// temporary infinite spawn function
	void TempSpawnRandom(){
		if (playerTarget != null && !spawnEnemy && !spawnMultiple) {
			spawnReset = true;
			int roll = Random.Range (0, 200);
			if (roll % 2 == 0) {
				spawnEnemy = true;
			} else {
				spawnMultiple = true;
			}
		}
	}

	void ResetSpawnReset(){
		spawnReset = false;
	}

	void SpawnEnemy(Vector3 spawn, EnemySpawnSide side = EnemySpawnSide.Right){
		List<Vector3> enemyNav = new List<Vector3> ();
		int rangeVal = Random.Range (0, 200);
		if (rangeVal % 2 == 0)
			AddTrianglePattern (spawn, enemyNav, spread, side);
		else
			AddSquarePattern (spawn, enemyNav, spread, side);

		Destroy (Instantiate (markerPrefab, enemySpawnPoint, Quaternion.identity), markerLifetime);

		foreach (Vector3 loc in enemyNav)
			Destroy (Instantiate (markerPrefab, loc, Quaternion.identity), markerLifetime);

		GameObject enemyInstance = Instantiate (enemyPrefab, enemySpawnPoint, Quaternion.identity) as GameObject;
		EnemyNavigator eiNav = enemyInstance.GetComponent<EnemyNavigator> ();
		eiNav.SetSpeed (enemySpeed);
		eiNav.PopulateDirectionList (enemyNav);
		eiNav.SetMoveDelay (enemyMoveDelay);
	}

	IEnumerator SpawnMultiple(int quantity){
		EnemySpawnSide side = RandomSideLR ();
		CalculateSpawnPos (side);
		GrabPlayerLocation ();
		Vector3 waveLocation = playerLocation;
		for (int i = 0; i < quantity; i++) {
			SpawnEnemy (OffsetSpawnPos(waveLocation, i), side);
			yield return new WaitForSeconds (spawnDelay);
		}
	}

	void ReferencePlayerTarget(){
		if (playerTarget == null)
			playerTarget = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
	}

	void CalculateSpawnPos(EnemySpawnSide side = EnemySpawnSide.Right){
		switch (side){
		case EnemySpawnSide.Right:
			enemySpawnPoint = new Vector3 (rightClearX - 2, playerTarget.position.y + spread, 5);
			break;
		default:
			// this will cover Left and Top for right now
			enemySpawnPoint = new Vector3 (leftClearX + 2, playerTarget.position.y + spread, 5);
			break;
		}
	}

	void GrabPlayerLocation(){
		playerLocation = playerTarget.position;
	}

	Vector3 OffsetSpawnPos(Vector3 original, int offsetNum = 0){
		Debug.Log ("offsetNum: " + offsetNum);
		float xOffset = sequentialXOffset * offsetNum;
		float yOffset = sequentialYOffset * offsetNum;
		return (original + new Vector3 (xOffset, yOffset, 0));
	}

	Vector3 DestinationCalc(Vector3 spawn, EnemySpawnSide side = EnemySpawnSide.Right){
		switch(side){
		case EnemySpawnSide.Left:
			return new Vector3(rightClearX, spawn.y+spread, 0);	// destination
			break;
		default:
			return new Vector3(leftClearX, spawn.y+spread, 0);	// destination
			break;
		}
	}

	void AddTrianglePattern(Vector3 spawn, List<Vector3> enemyNav, float spread, EnemySpawnSide side = EnemySpawnSide.Right){
		// multiply by -1 to invert values for left
		int invert = 1;
		if (side == EnemySpawnSide.Left)
			invert = -1;

		enemyNav.Add(spawn + new Vector3(-spread * invert, spread, 0));		// point 1 
		enemyNav.Add(spawn + new Vector3(0, spread*2, 0));					// point 2 (top)
		enemyNav.Add(spawn + new Vector3(spread * invert, spread, 0));		// point 3
		enemyNav.Add(DestinationCalc(spawn, side));
	}

	void AddSquarePattern(Vector3 spawn, List<Vector3> enemyNav, float spread, EnemySpawnSide side = EnemySpawnSide.Right){
		// multiply by -1 to invert values for left
		int invert = 1;
		if (side == EnemySpawnSide.Left)
			invert = -1;
		
		enemyNav.Add(spawn + new Vector3(-spread * invert, spread, 0));		// point 1 
		enemyNav.Add(spawn + new Vector3(-spread * invert, spread*3, 0));	// point 2 (top 1)
		enemyNav.Add(spawn + new Vector3(spread * invert, spread*3, 0));	// point 3 (top 2)
		enemyNav.Add(spawn + new Vector3(spread * invert, spread, 0));		// point 4
		enemyNav.Add(DestinationCalc(spawn, side));
	}
}

