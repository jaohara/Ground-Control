using UnityEngine;
using System.Collections;

public class Blaster : Gun {

	public Blaster (){
		gunListSpot = 0;
		speed = 30f;
		shotDamage = 34;
		gunSpread = 0.05f;
		shotDelay = 0.2f;
		burstAmount = 1;
		burstDelay = 0.0f;
	}
}

