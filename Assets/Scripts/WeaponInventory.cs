using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for the list

public class WeaponInventory : MonoBehaviour {

	/*
	 * Just going to spitball some clairvoyance here, I think I'm going to have a problem 
	 * with coding the weapon pickups. Presumably I'd make a pickup with a reference to 
	 * the prefab for each of the guns, but I'm wondering what will happen when I pass 
	 * that prefab into the gunInventory list. Wait, will this be a problem? I instantiate 
	 * the prefab by instantiating the gameObject attached to the gun component stored in 
	 * the gun Inventory. This might be easier than I think it will be, but I'm going to keep
	 * this note here so I can remember my train of thought on the potential problem should
	 * I run into it further down the line.
	 * 
	 * - 4/11/16 1:21 PM
	 */

	public List<Gun> gunInventory;						// the actual gun inventory as a List of <Gun>s
	public int inventoryIndex;							// where are we in the inventory?
	public Gun equipped;								// the currently equipped gun to be instantiated

	public Gun currentGunObject;						// the currently instantiated gun

	public GunController gunController;					// reference to the player's guncontroller

	private bool switchingGuns;							// are we already switching?

	void Start () {
		switchingGuns = false;
		inventoryIndex = 0;
		EquipGun ();
	}

	void Update () {
		ChangeGunCheck ();
	}

	void ChangeGunCheck(){
		/*
		 * I think there's a prettier way to write this, condensing it into a single if 
		 * statement and determining which direction to go based on some intrinsic property
		 * of each button. Maybe make them an axis and have one equal negative so the value
		 * could just be passed to ChangeGun() and find the proper direction?
		 */
		if (Input.GetButtonDown ("NextGun")  && !switchingGuns) {
			ChangeGun (1.0f);
			switchingGuns = true;
		} else if (Input.GetButtonDown ("PrevGun") && !switchingGuns) {
			ChangeGun (-1.0f);
			switchingGuns = true;
		}

		if (Input.GetButtonUp ("NextGun") || Input.GetButtonUp ("PrevGun")) {
			if (!Input.GetButton ("NextGun") && !Input.GetButton ("PrevGun"))
				switchingGuns = false;
		}
	}

	// this and above could probably be more elegant
	void ChangeGun(float inventoryDirection){
		if (inventoryDirection > 0) {
			inventoryIndex = inventoryIndex >= (gunInventory.Count-1) ? 0 : inventoryIndex + 1;
		} else if (inventoryDirection < 0) {
			inventoryIndex = (inventoryIndex - 1) < 0 ? gunInventory.Count - 1 : inventoryIndex - 1;
		}

		EquipGun ();
	}

	void EquipGun(){
		if (currentGunObject != null) {
			Destroy (currentGunObject.gameObject);
		}

		equipped = gunInventory [inventoryIndex];

		gunController.equipped = equipped;

		currentGunObject = (Gun)Instantiate (equipped, gunController.gunSpawnTrans.position, gunController.gunSpawnTrans.rotation);

		currentGunObject.gameObject.transform.parent = gameObject.transform;
		gunController.SetGunBarrelTrans (currentGunObject.GetComponentInChildren<Transform> ());
	}
}
