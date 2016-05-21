using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathMoverIdea : MonoBehaviour {

	public List<Vector3> directionList;					// List of Vector3 destinations
	public int dumbIndex;								// this is stupid, there's probably a better way. Which list index are we at?
	public float speed;									// speed modifier of the thing being moved
	public float moveDelay = 0.0f;						// how many seconds to wait between movements?
	public bool useSlerpForNavigation = false;			// Do i use slerp or lerp for navigation?

	private Vector3 startPos;							// original position to offset the directions
	private Rigidbody theRb;							// the rigidbody on the gameobject
	private bool willMove;								// have we registered a move action and will we move during fixedupdate?
	private bool onPath = false;						// are we already moving? prevent willMove from being true
	private bool waitLocked = false;					// are we still waiting to be able to move?



	void Start () {
		theRb = gameObject.GetComponent<Rigidbody> ();
		dumbIndex = 0;
		startPos = theRb.position;
		OffsetY ();
		//OffsetDirections ();
		//willMove = true;
	}

	void Update(){
		if (!onPath && !waitLocked)
			willMove = true;
	}

	void FixedUpdate () {
		if (willMove) {
			StartCoroutine ("Navigate");

			willMove = false;
		}
	}

	void OffsetDirections(){
		for (int i = 0; i < directionList.Count; i++) {
			directionList [i] = directionList [i] + startPos;
		}
	}

	void OffsetY(){
		for (int i = 0; i < directionList.Count; i++) {
			directionList [i] = directionList [i] + new Vector3(0.0f, startPos.y, 0.0f);
		}
	}

	void WaitForNextStep(){
		waitLocked = false;
	}

	public IEnumerator Navigate(){
		waitLocked = true;
		onPath = true;

		/*
		 * Okay, I've got a cool idea here (and maybe the solution?) but some poorly named variables are 
		 * making it difficult to go back and figure out how I made this work. 
		 * 
		 * Basically, i take the distance from the start to the destination, then divide 1.0 by this to 
		 * get the distance of a single step. This works to lock the speed of the object in question. It will
		 * move that certain percentage of the distance each time it iterates through the for loop. This 
		 * works to lock the speed to a certain rate regardless of distance being traveled.
		 * 
		 * Think like this: before we had ten steps to cover an arbitrary distance. The bigger the distance,
		 * the bigger one-tenth of it is, which results in a larger distance being covered each step, and thus 
		 * the object is moving at a greater speed. With the new method, the number of steps increases as the 
		 * distance does, since we divide 1 by the distance to get the percentage of the distance closed in each 
		 * step. A distance of 5 makes a step of .2, making the distance covered in 5 steps. A distance of 10 
		 * makes a step of .1, making the distance covered in 10 steps. 10/10 = 1 = 5/5, our speed is locked 
		 * no matter the distance.
		 */
		Vector3 startPos = theRb.position;
		float distanceVar = Vector3.Distance (startPos, directionList [dumbIndex]);
		float distanceUnit = 1.0f / distanceVar;
		float moveStartTime = Time.timeSinceLevelLoad;
		/* // these are all for debug purposes
		Debug.Log("Distance: " + distanceVar.ToString());
		Debug.Log ("Unit: " + distanceUnit);
		*/

		for (float i = 0.00f; i <= 1.0f; i+=(distanceUnit*speed)/10.0f){
			if (useSlerpForNavigation)
				theRb.MovePosition(Vector3.Slerp (startPos, directionList [dumbIndex], i));
			else
				theRb.MovePosition(Vector3.Lerp (startPos, directionList [dumbIndex], i)); 
			yield return null;
		}

		if (dumbIndex + 1 >= directionList.Count)
			dumbIndex = 0;
		else
			dumbIndex++;

		/* // more debug calls
		float moveTime = Time.timeSinceLevelLoad - moveStartTime;
		float calcSpeed = distanceVar/moveTime;
		Debug.Log ("Movement completed in " + moveTime.ToString() + "ms");
		Debug.Log ("Speed: " + calcSpeed.ToString () + " Units/ms");
		*/

		onPath = false;
		Invoke ("WaitForNextStep", moveDelay);
	}
}
