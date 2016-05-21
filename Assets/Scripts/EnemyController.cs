using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour, IKillable, IDamageable {

	public GameObject killPrefab;			// object to be instantiated on death
	public int scoreValue;					// value for a kill

	public int health;						// the object health

	public int enemyCollideDamage;			// damage upon contact with player

	public GameController gameController;	// the gamecontroller

	private Rigidbody enemyRb;				// this object's rb

	private ParticleSystem enemyTrail;		// the particle trail emitted by this object
	private Material enemyMaterial;			// this object's material
	private Color baseColor;				// this object's albedo

	void Start () {
		GameObject gameControllerObj = GameObject.FindGameObjectWithTag ("GameController");

		if (gameControllerObj != null)
			gameController = gameControllerObj.GetComponent<GameController> ();
		else
			Debug.Log ("Error: Cannot find 'GameLogic' script");

		enemyRb = gameObject.GetComponent<Rigidbody> ();
		enemyMaterial = gameObject.GetComponent<MeshRenderer> ().material;
		baseColor = enemyMaterial.color;

		enemyTrail = gameObject.GetComponentsInChildren<ParticleSystem> ()[0];
	}

	void OnTriggerEnter(Collider contact){
		/* THIS IS ALSO IN THE PROJECTILECONTROLLER. SHOULD THIS BE MADE
		 * INTO AN INTERFACE LIKE "DOES DAMAGE"....ABLE?
		 */
		//check to see if it hit the player, if so do some damage
		Debug.Log("Collision hit!");

		if (contact.CompareTag ("Player")) {
			MonoBehaviour contactMonoBehaviour = contact.gameObject.GetComponent<MonoBehaviour> ();
			if (contactMonoBehaviour != null && contactMonoBehaviour is IDamageable) {
				Debug.Log ("EOCE: Target is IDamageable.");
				IDamageable contactD = contactMonoBehaviour as IDamageable;
				contactD.Damage (enemyCollideDamage);
			} else
				Debug.Log ("EOCE: Target IS NOT IDamageable.");
		}

		// have the ground destroy the enemy
		if (contact.CompareTag ("Environment")) {
			DestroyWithExplosion ();
		}
	}

	void ResetColor(){
		enemyMaterial.color = baseColor;
	}

	public void Damage(int damageTaken){
		health -= damageTaken;
		if (health <= 0)
			Kill ();

		enemyMaterial.color = Color.white;
		Invoke ("ResetColor", 0.05f);
	}

	public void DestroyWithExplosion(){
		DetachParticles ();
		Destroy (Instantiate (killPrefab, enemyRb.position, enemyRb.rotation), 1.25f);
		Destroy (gameObject);
	}

	public void Kill(){
		gameController.Score = scoreValue;
		PickupDropper pd = gameObject.GetComponent<PickupDropper> ();
		if (pd != null)
			pd.SpawnPickup ();
		DestroyWithExplosion ();
	}

	public void DetachParticles(){
		enemyTrail.transform.parent = null;
		Destroy (enemyTrail, enemyTrail.startLifetime);
	}
}
