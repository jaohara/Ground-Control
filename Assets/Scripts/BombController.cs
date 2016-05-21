using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombController : MonoBehaviour {

	public int bombs = 0;
	public int maxBombs = 5;
	public float bombCoolDown = 5.0f;

	public GameObject explosionPrefab;

	private bool willExplode = false;
	private bool bombArmed = true;

	private CameraZoomer cameraZoom;

	void Start () {
		cameraZoom = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraZoomer> ();

		// make this not hard-coded
		bombs = 3;
	}

	void Update () {
		if (willExplode) {
			if (bombs > 0 && bombArmed) {
				Explode ();
				Invoke ("RearmBomb", bombCoolDown);
			} 
			willExplode = false;
		}
	}

	void Explode(){
		// explode the bomb
		//search for all enemies on screen
		PlayerController.Instance.bombSound.Play();
		cameraZoom.RubberBandZoom ();
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag ("Projectile");
		foreach (GameObject enemy in enemies) {
			EnemyController ec = enemy.GetComponent<EnemyController> ();
			if (ec != null)
				ec.Kill ();
		}
		foreach (GameObject projectile in projectiles) {
			ProjectileController pc = projectile.GetComponent<ProjectileController> ();
			if (pc != null) {
				if (pc.spawnerTag == "Enemy")
					Destroy (pc.gameObject);
			}
		}

		bombs--;
		bombArmed = false;
	}

	public void TriggerExplosion(){
		if (!willExplode)
			willExplode = true;
	}

	public void RearmBomb(){
		bombArmed = true;
	}

	public void AddBombs(int number){
		if (bombs < maxBombs)
			bombs += number;
	}
}
