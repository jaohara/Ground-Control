using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyNavigator : MonoBehaviour
{
	public List<Vector3> directionList;					// List of Vector3 destinations
	public float speed;									// speed modifier of the enemy being moved
	public float moveDelay = 0.0f;						// how many seconds to wait between movements?
	public bool useSlerpForNavigation = false;			// Do I use slerp or lerp for navigation?

	private int directionIndex = 0;						// Which list index are we at?
	private Vector3 startPos;							// original position to offset the directions
	private Rigidbody theRb;							// the rigidbody on the gameobject
	private bool willMove;								// have we registered a move action and will we move during fixedupdate?
	private bool onPath = false;						// are we already moving? prevent willMove from being true
	private bool waitLocked = false;					// are we still waiting to be able to move?

	void Start () {
		theRb = gameObject.GetComponent<Rigidbody> ();
		startPos = theRb.position;
		// I don't know why I thought this was so important but I'm going to keep it around. It is causing problems in its current state.
		//OffsetY ();
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

	public void AddDirection(Vector3 newDirection){
		directionList.Add (newDirection);
	}

	public void PopulateDirectionList(List<Vector3> replacementList){
		directionList = replacementList;
		directionIndex = 0;
	}

	public void SetMoveDelay(float newDelay){
		moveDelay = newDelay;
	}

	public void SetSpeed(float newSpeed){
		speed = newSpeed;
	}

	void OffsetDirections(){
		for (int i = 0; i < directionList.Count; i++) {
			directionList [i] = directionList [i] + startPos;
		}
	}

	void OffsetY(){
		for (int i = 0; i < directionList.Count; i++) {
			//directionList [i] = directionList [i] + new Vector3 (0.0f, startPos.y, 0.0f);
			directionList [i] = new Vector3 (directionList[i].x, directionList[i].y+startPos.y, directionList[i].z);
		}
	}

	void WaitForNextStep(){
		waitLocked = false;
	}

	public IEnumerator Navigate(){
		waitLocked = true;
		onPath = true;

		Vector3 startPos = theRb.position;
		float distanceVar = Vector3.Distance (startPos, directionList [directionIndex]);
		float distanceUnit = 1.00000f / distanceVar;
		float moveStartTime = Time.timeSinceLevelLoad;

		for (float i = 0.00f; i <= 1.0f; i+=(distanceUnit*speed)/10.00000f){
			if (useSlerpForNavigation)
				theRb.MovePosition(Vector3.Slerp (startPos, directionList [directionIndex], i));
			else
				theRb.MovePosition(Vector3.Lerp (startPos, directionList [directionIndex], i)); 
			yield return null;
		}

		if (directionIndex + 1 >= directionList.Count)
			directionIndex = 0;
		else
			directionIndex++;

		onPath = false;
		Invoke ("WaitForNextStep", moveDelay);
	}
}

