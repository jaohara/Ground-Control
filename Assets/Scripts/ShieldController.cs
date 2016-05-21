using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour, IDamageable, IHealable {

	public int power;						// current shield power
	public int maxShield;					// maximum shield power
	public float shieldDecayRate;			// subtract 1 from shield how quickly?
	public float shieldHealRate;			// recharge shield how quickly?
	public float shieldHealThreshold;		// how long before I start recharging?
	private bool healInProgress = false;	// have we already begun healing?
	[HideInInspector]
	public bool shieldActive;				// is the shield activated?

	[HideInInspector]
	public float timeSinceLastUse;			// how long since we've last used the shield?
	private float shieldTimer;				// used for decay/recharge

	[HideInInspector]
	public PlayerController player;			// the playerController using the shield

	void Start () {
		//timeSinceLastDecay = 0.0f;
		shieldDecayRate = 0.05f;
		shieldHealRate = 0.1f;
		shieldHealThreshold = 3.0f;
		shieldTimer = 0.0f;

		maxShield = 200;
		power = maxShield;
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null)
			shieldActive = player.shieldActive;

		if (shieldActive) {
			PlayShieldNoise ();
			timeSinceLastUse = Time.timeSinceLevelLoad;
			DecayShield ();
		} else {
			StopShieldNoise ();
			shieldTimer = Time.timeSinceLevelLoad;
			RechargeShield ();
		}

		Debug.Log ("" + shieldActive + ": " + power);
	}

	public void DecayShield (){
		//timeSinceLastDecay = Time.timeSinceLevelLoad - shieldTimer;

		if (Time.timeSinceLevelLoad - shieldTimer >= shieldDecayRate) {
			Damage (1);
			shieldTimer = Time.timeSinceLevelLoad;
		}
	}
		
	public void RechargeShield(){
		if (shieldTimer - timeSinceLastUse >= shieldHealThreshold) {
			if (!healInProgress && (power < maxShield)) {
				Heal (1);
				healInProgress = true;
				Invoke ("ResetHealState", shieldHealRate);
			}
		}
	}

	void ResetHealState(){
		healInProgress = false;
	}

	void PlayShieldNoise(){
		if (!PlayerController.Instance.shieldSound.isPlaying) {
			PlayerController.Instance.shieldSound.Play ();
		}
	}

	void StopShieldNoise(){
		if (PlayerController.Instance.shieldSound.isPlaying) {
			PlayerController.Instance.shieldSound.Stop ();
		}
	}

	public void Damage (int damageTaken){
		if (power - damageTaken <= 0) {
			//calculate the damage bled through the shield, then apply to player
			shieldActive = false;
			player.shieldActive = false;
			player.Damage (Mathf.Abs(power - damageTaken));
			power = 0;
		} else {
			power -= damageTaken;
			//add some score based on absorbing a shot?
			// need to add a reference to my gameController
		}
	}

	public void Heal (int damageHealed){
		//power += damageHealed;
		power = (power + damageHealed) > maxShield ? maxShield : power + damageHealed;

		//player.ShieldHealColor ();
	}
}
