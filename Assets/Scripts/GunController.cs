using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour
{
	public bool controllerLook;					// look with controller or mouse?
	private bool canShoot;

	// object references
	public Transform gunSpawnTrans;
	public Transform gunBarrelTrans;			// trans for the gun barrel
	public RectTransform crosshair;				// the recttransform containing the crosshair
	public RectTransform[] aimingCircles;		// array for rect transforms of aiming circles

	public float crosshairDistance;				// how far is the crosshair drawn from the gunbarreltrans?

	public Vector3 shootDirection;				// a vector to store the rightstick/mouse direction	
	/*public Vector3 ShootDirection {
		get;
		set;	// does this open vulnerability or something? I had this read-only by default
	}*/

	private Quaternion shootRotation;			// the quaternion rotation of the aiming angle
	public Quaternion ShootRotation{
		get { return shootRotation; }
	}
		
	// this should eventually be hidden, but it's going to be public for testing
	public Gun equipped;

	void Start (){
		controllerLook = false;
		crosshairDistance = 9.0f;

		Invoke ("ResetShot", 0.1f); // this is kinda arbitrary but should fix the bug
	}

	void Update (){
		//LookCheck ();
		if (equipped != null) {
			CalculateSpread ();
			DrawCrosshair ();

			//ShootCheck ();
		}
	}



	void CalculateSpread(){
		Vector3 spreadDirection = 
			new Vector3 (shootDirection.x + Random.Range ((-1) * equipped.gunSpread, equipped.gunSpread),
				shootDirection.y + Random.Range ((-1) * equipped.gunSpread, equipped.gunSpread),
				shootDirection.z + Random.Range ((-1) * equipped.gunSpread, equipped.gunSpread));

		shootRotation = Quaternion.LookRotation (spreadDirection, Vector3.back);
	}

	void DrawCrosshair(){
		crosshair.position = gunBarrelTrans.position + (shootDirection.normalized * crosshairDistance);

		for (int i = 0; i < aimingCircles.Length; i++) {
			aimingCircles[i].position = gunBarrelTrans.position + (shootDirection.normalized * (crosshairDistance / (i + 1.5f)));
		}
	}

	void ShootCheck(){
		if (Input.GetAxis ("Fire1") > 0.0)
			StartCoroutine ("Shoot");
	}

	/*
	 * This seems like it might be able to be made into an interface, because I'm about to 
	 * reuse some of this code for the EnemyShooter
	 */

	// also, weird error - first shot if held down on spawn floats with no velocity. look into that.
	IEnumerator Shoot(){
		if (canShoot) {
			canShoot = false;
			// make our burst loop
			for (int i = 0; i < equipped.burstAmount; i++) {
				//AudioSource test = equipped.gameObject.GetComponent<AudioSource> ();
				//Debug.Log (test);
				PlayerController.Instance.shotSound.Play();
				GameObject thisShot = (GameObject)Instantiate (equipped.projectilePrefab, gunBarrelTrans.position, shootRotation);
				ProjectileController shotController = thisShot.GetComponent<ProjectileController> ();
				shotController.spawnerTag = gameObject.tag;
				shotController.speed = equipped.speed;
				shotController.shotDamage = equipped.shotDamage;
				shotController.GetComponent<Rigidbody>().velocity = 
					shotController.transform.forward * shotController.speed;
				yield return new WaitForSeconds(equipped.burstDelay); 
			}
			//end the burst loop
			Invoke ("ResetShot", equipped.shotDelay);
		}
	}

	void ResetShot(){
		canShoot = true;
	}

	public void SwapLookControl(){
		controllerLook = !controllerLook;
	}

	public void SetGunBarrelTrans(Transform trans){
		gunBarrelTrans = trans;
	}
}

/*
 * Graveyard for deprecated input methods - RIP
 */

/*

void LookCheck(){
	Vector3 frameShootDirection;

	if (controllerLook) {
		frameShootDirection = Vector3.right * Input.GetAxis ("RSHorizontal") + Vector3.up * Input.GetAxis ("RSVertical");
	} else {
		frameShootDirection = Input.mousePosition;
		frameShootDirection.x -= Screen.width / 2;
		frameShootDirection.y -= Screen.height / 2;
	}

	if (frameShootDirection.sqrMagnitude >= 0.1f) 
		shootDirection = frameShootDirection;

	//Debug.Log ("fsd: " + frameShootDirection + ", sd: " + shootDirection);
}
*/