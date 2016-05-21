using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour {

	public PlayerController target;			// the target to shoot at

	public GameObject projectilePrefab;		// which projectile is instantiated?
	public int shotDamage;					// how much damage does this do to a player?
	public float shotSpeed;					// how fast is our shot?
	public float shootDelay;				// how many seconds between shots?

	private bool canShoot;					// can the enemy shoot this frame?

	void Start (){
		ResetShoot ();

		/* Hardcoded, to be replaced
		shotDamage = 34;
		shotSpeed = 8.0f;
		shootDelay = 1.25f;
		*/
	}

	void Update () {
		if (target == null) {
			// find the target
			GameObject tempTarget = GameObject.FindGameObjectWithTag("Player") as GameObject;
			canShoot = false;

			if (tempTarget != null) {
				target = tempTarget.GetComponent<PlayerController> ();
				Invoke ("ResetShoot", shootDelay);
			}
			//Debug.Log ("There is no player target.");
		}

		if (target != null) {
			if (canShoot) {
				Shoot ();
			}
			//Debug.Log("We have found a player target.");
		}
	}

	/*
	 * For the most part this is very similar to the Shoot() coroutine in my GunController. I should
	 * work to combine these ideas into a more generic "Shootable" interface (maybe Fireable? shootable
	 * sort of implies that it's being hit by the shot)
	 */
	void Shoot(){
		if (target == null)
			Debug.LogError ("Error: No target for this EnemyShooter Shoot method.");
	
		canShoot = false;
		GameObject thisShot = 
			(GameObject)Instantiate (projectilePrefab, transform.position + new Vector3(0f, -1.5f, 0f), transform.rotation);
		ProjectileController shotController = thisShot.GetComponent<ProjectileController> ();
		shotController.spawnerTag = gameObject.tag;
		shotController.speed = shotSpeed;
		shotController.shotDamage = shotDamage;
		shotController.gameObject.transform.LookAt (target.gameObject.transform);


		// right now this won't work as the shot isn't rotated to face the player
		shotController.GetComponent<Rigidbody> ().velocity = 
			shotController.transform.forward * shotController.speed;

		/*
		shotController.GetComponent<Rigidbody> ().velocity = 
			Vector3.down * shotController.speed;
		*/

		Invoke ("ResetShoot", shootDelay);
	}

	void ResetShoot(){
		canShoot = true;
	}
}
