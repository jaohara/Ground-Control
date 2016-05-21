using UnityEngine;
using System.Collections;

public interface IKillable{
	void Kill();
}

public interface IDamageable{
	void Damage (int damageTaken);
}

// I like what I did here! something can be damageable but not healable, but if something is healable
// it needs to also be damageable. This was some cool foresight, past me.
public interface IHealable : IDamageable{
	void Heal (int damageHealed);
}
	
public interface ICollectable{
	// I don't really know what I'm doing with this here?
	void Collect (PlayerController collector);
}


//should I have this here or in another enum script?
public enum PickupType{Health,Shield,Life,Score,Bomb}
public enum MovementType{Linear, Sine}				
public enum MovementPattern{Linear, Triangle, Square, Hexagon, Octagon, Sphere}
public enum EnemySpawnSide{Right, Left, Top}