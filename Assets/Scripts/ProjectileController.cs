using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

	public float speed;								// moving speed of the projectile
	public float shotDelay;							// delay before another shot
	public int shotDamage;							// amount of damage this projectile deals to IDamageable

	public GameObject destroyPrefab;				// prefab for projectile destruction

	public Rigidbody projectileRb;					// rigidbody for the projectile

	public string spawnerTag;						// tag of who spawned it

	void Start () {
	}

	void OnTriggerEnter(Collider contact){
		/*
		 * This is a real hacky mess, but I think I figured out what I need to.
		 * 
		 * We first make sure the contact isn't of the same tag as the thing that spawned it. So no
		 * enemy bullets hitting enemies, no player bullets hitting players.
		 * 
		 * We then grab the MonoBehaviour component of the gameObject attached to the collider,
		 * which will grab the first script component that we've written and attached to 
		 * the gameObject.
		 * 
		 * THIS IS ALWAYS THE FIRST. AS A RESULT, SCRIPT IMPLEMENTING THE INTERFACE MUST BE 
		 * THE TOPMOST COMPONENT (USUALLY OUR CONTROLLER SCRIPT)
		 * 
		 * We make sure the monobehaviour object is not null and then check to see if it 
		 * "is IDamageable", that is to say implements the IDamageable interface.
		 * 
		 * If we pass all of these checks, we're in the home stretch. Just cast it to 
		 * an IDamageable and call the Damage method with the proper amount of damage 
		 * to be received.
		 */

		// some sort of way to handle hitting something that does have a tag? give everything a tag?
		if (!contact.CompareTag (spawnerTag) && !contact.CompareTag(gameObject.tag)) {
			Destroy (Instantiate (destroyPrefab, gameObject.transform.position, gameObject.transform.rotation), 0.4f);
			Destroy (gameObject);
			MonoBehaviour contactMonoBehaviour = contact.gameObject.GetComponent<MonoBehaviour> ();
			//Debug.Log (contactMonoBehaviour);
			if (contactMonoBehaviour != null && contactMonoBehaviour is IDamageable) {
				Debug.Log ("Target is IDamageable.");
				IDamageable contactD = contactMonoBehaviour as IDamageable;
				contactD.Damage (shotDamage);
			} else
				Debug.Log ("Target IS NOT IDamageable.");
		}
	}
}
