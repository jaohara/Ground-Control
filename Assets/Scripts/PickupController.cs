using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour, ICollectable {

	public PickupType pickupAffects;
	public GameObject pickupEffectPrefab;

	public int value;

	void OnTriggerEnter(Collider col){
		// check to see if the collider's gameobject has a playercontroller. if so, pass to Collect
		PlayerController pc = col.gameObject.GetComponent<PlayerController>();
		if (pc != null)
			Collect (pc);
	}

	void OnCollisionEnter(Collision col){
		// check to see if the collider's gameobject has a playercontroller. if so, pass to Collect
		PlayerController pc = col.gameObject.GetComponent<PlayerController>();
		if (pc != null)
			Collect (pc);
	}

	public void Collect (PlayerController collector){
	
		/* this is going to be really shitty and ugly for now. we're going to be checking for the 
		 * type of pickup twice, once in this script and once in the next script. I don't really 
		 * like how redundant this method will be.
		 * 
		 * It might be worth removing the CollectPickup() method in the PlayerController and just
		 * directly calling whichever PlayerController method the type hooks to from within this 
		 * function. I think I'm going to explore this method before I make the double check
		 * 
		 */

		switch (pickupAffects){
		case PickupType.Health:
			// health
			if (collector.health != collector.maxHealth) {
				collector.Heal (value);
				DestroyWithEffect();
			}
			break;
		case PickupType.Shield:
			// shield
			if (collector.playerShield.maxShield != collector.playerShield.power) {
				collector.playerShield.Heal (value);
				DestroyWithEffect();
			}
			break;
		case PickupType.Bomb:
			collector.playerBombs.AddBombs (value);
			DestroyWithEffect ();
			break;
		case PickupType.Life:
			collector.ChangeLives (value);
			DestroyWithEffect();
			break;
		case PickupType.Score:
			// no check, just add
			collector.ModifyScore (value);
			DestroyWithEffect();
			break;
		default:
			// whatever, score for now. this makes this case redundant
			collector.ModifyScore (value);
			DestroyWithEffect();
			break;
		}

	}

	public void DestroyWithEffect(){
		Destroy (transform.gameObject);
		GameObject.FindGameObjectWithTag ("AudioController").GetComponents<AudioSource> () [6].Play ();
		// this is some hard coded grossness
		Destroy(Instantiate (pickupEffectPrefab, transform.position, transform.rotation), 0.5f);
	}
		
}
