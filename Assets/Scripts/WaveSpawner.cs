using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
	public List<WaveBehavior> waves;						// the waves to spawn
	public int waveIndex;									// which wave?
	[HideInInspector]
	public Vector3 spawnerPosition;							// where do we spawn from?

	public bool repeatWaves;								// do we repeat when finished?

	private bool waveInProgress;							// are we wavin' it already?

	void Start (){
		spawnerPosition = gameObject.transform.position;
		waveInProgress = false;
		waveIndex = 0;
	}

	void Update () {
		if (!waveInProgress) {
			StartCoroutine ("SpawnWave");
		}
	}



	/*
	 * NONE OF THIS HAS BEEN TESTED TO WORK. I'M STILL WRITING IT
	 * 
	 * I'm feeling kind of fried and can't keep track of what it is I'm supposed to be writing right now.
	 * This function is what processes and spawns a Wave according to a given WaveBehavior, which stores
	 * a list of all of the enemies to spawn as part of the wave as well as the parameters for the wave
	 * like the delay and the enemy speed. so far I've got it to check if we have a WaveBehavior, 
	 * then start a loop to go through each element of the WaveBehavior's waveEnemies List and 
	 * instantiate them at a random y position from the spawner based on the waveYOffset field specified
	 * on the WaveBehavior, then we yield a waitForSeconds equal to the number of seconds between
	 * each enemy spawn. 
	 * 
	 * That's about all I have right now, I can't do any more of this.
	 */
	IEnumerator SpawnWave(){
		waveInProgress = true;
		if (waves [waveIndex] != null) {
			//start spawning the wave
			for (int i = 0; i < waves [waveIndex].waveEnemies.Count; i++) {
				//we're in the waveEnemies list

				// put a little offset on the Vector position of the spawner
				Vector3 spawnVector = spawnerPosition 
					+ new Vector3(0,Random.Range(waves[waveIndex].waveYOffset*-1, waves[waveIndex].waveYOffset), 0);

				Instantiate (waves [waveIndex].waveEnemies [i], spawnVector, Quaternion.identity);
				yield return new WaitForSeconds (waves [waveIndex].spawnDelay);
				// HERE'S WHERE I STOPPED
			}
			yield return new WaitForSeconds (waves [waveIndex].waveDelay);
		}
		waveIndex++;
		if (waveIndex == waves.Count && repeatWaves)
			waveIndex = 0;
		waveInProgress = false;
	}
}

