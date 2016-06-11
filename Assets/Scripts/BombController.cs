using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombController : MonoBehaviour {

	public int maxBombs = 5;
	public float bombCoolDown = 1.0f;

	public GameObject explosionPrefab;

	private bool willExplode = false;
	private bool bombArmed = true;

	private CameraZoomer cameraZoom;
	private Animator playerUIAC;

	void Start () {
		cameraZoom = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraZoomer> ();
		playerUIAC = GameObject.FindGameObjectWithTag ("PlayerStatusUI").GetComponent<Animator> ();
	}

	void Update () {
		if (willExplode) {
			if (GameController.Instance.Bombs > 0 && bombArmed) {
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

		GameController.Instance.Bombs = -1;
		bombArmed = false;
	}

	public void TriggerExplosion(){
		if (!willExplode)
			willExplode = true;
	}

	public void RearmBomb(){
		bombArmed = true;
		if (playerUIAC != null && GameController.Instance.Bombs != 0) {
			playerUIAC.SetBool ("BombActive", true);
			Invoke ("ResetBombAnimation", 0.5f);
		}
	}

	//this is pretty hacky, but my animation trigger isn't consistently working
	public void ResetBombAnimation(){
		if (playerUIAC != null)
			playerUIAC.SetBool("BombActive", false);
	}

	public void AddBombs(int number){
		if (GameController.Instance.Bombs < maxBombs)
			GameController.Instance.Bombs = number;
	}
}
