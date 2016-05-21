using UnityEngine;
using System.Collections;

public class Shotgun : Gun {
	
	public Shotgun (){
		gunListSpot = 2;
		speed = 45.0f;
		shotDamage = 34;
		gunSpread = 4.0f;
		shotDelay = 1.0f;
		burstAmount = 6;
		burstDelay = 0.1f;
	}
}

