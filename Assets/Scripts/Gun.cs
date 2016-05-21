using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour, System.IComparable<Gun> {

	/*
	 * Right now this can even be further broken down into a GunController (looking, shooting, taking
	 * the spread data and shotdelay from the actual gun data object) and a GunData (probably just using
	 * this class name) object which stores the ammo, values for shotdelay, damage, projectile type
	 */

	public int gunListSpot;							// used to list guns in order, think quake hotkeys

	public RectTransform crosshair;					// MODIFY INTO GUN'S SPECIFIC CROSSHAIR SPRITE

	public GameObject projectilePrefab;				// the prefab for the projectile

	public float speed;								// how fast do the projectiles travel?
	public int shotDamage;							// how much damage to IDamageable?

	public float gunSpread;							// spread for the projectiles as a float
	public float shotDelay;							// how long before you can shoot again?

	public int burstAmount;							// how many bullets are fired on a click?
	public float burstDelay;						// how much wait between each sequential shot?

	private int ammo;
	public int Ammo{
		/*
		 * I might need to look into defining a type or something for managing which ammo is which
		 */
		get{ return ammo; }
		set{ ammo += value; }
	}
		
	/*
	 * This int CompareTo(<T>) method is how we implement the IComparable<T> interface.
	 * negative value means this one is sorted earlier, 0 means it's sorted similarly, and
	 * a positive value means it's sorted after.
	 */

	public int CompareTo(Gun other){
		if (other == null)
			return 1;

		return gunListSpot - other.gunListSpot;
	}
}
