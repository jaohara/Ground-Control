using UnityEngine;
using System.Collections;

public class EnemyMovementPattern : MonoBehaviour
{
	public float enemySpeed;							// speed of the enemy when created
	public float movementAngle;							// what angle from the spawn?
	public float sineMagnitude;							// magnitude of the Sine wave?	(height of waves)
	public float sineFrequency;							// frequency of the sine wave? (speed)
	public Vector3 moveDirection;						// which direction will the enemy move?

	public MovementType enemyMovetype;					// how is this enemy going to move?

	private float creationTime;							// Time.timeSinceLevelLoad at creation
	private Rigidbody enemyRb;							// rigidbody of the enemy

	void Start (){
		enemyRb = gameObject.GetComponent<Rigidbody> ();
		creationTime = Time.timeSinceLevelLoad;
	}

	void Update (){
		if (enemyMovetype == MovementType.Sine)
			MoveSine ();
		else if (enemyMovetype == MovementType.Linear)
			MoveLinear ();
	}

	void MoveLinear(){
		Vector3 linearMoveVelocity = Quaternion.Euler(0,0,movementAngle) * moveDirection * enemySpeed;
		enemyRb.velocity = linearMoveVelocity;
	}

	void MoveSine(){
		float calcYPos = (Mathf.Sin ((Time.timeSinceLevelLoad-creationTime) * sineFrequency) * sineMagnitude) * Time.deltaTime;
		Vector3 sineMovePosition = 
			new Vector3(enemyRb.position.x + (enemySpeed * Time.deltaTime * moveDirection.x), 
			enemyRb.position.y + calcYPos, 
			enemyRb.position.z);
		enemyRb.transform.position = sineMovePosition;
	}
}
