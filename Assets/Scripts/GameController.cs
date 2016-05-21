using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// learning how to make this motherfucker a singleton

	#region SINGLETON PATTERN //what does this do?
	//1. make a static instance.
	public static GameController instance = null;

	//2. make an accessor with only the get property. 
	// this is where some real convenient magic comes into play - now i can refer to this instance globally.
	public static GameController Instance{
		get { return instance; }
	}

	void ActivateSingleton(){
		if (instance){
			DestroyImmediate(gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	#endregion

	private int score;								//really?
	public int Score {
		get{ return score; }
		set{ score += value; }
	}

	public GameObject playerPrefab;					// prefab of the player to be spawned.
	public Transform spawnPoint;					// trans of the spawn point.
	[HideInInspector]
	public GameObject player;						// instantiated player
	public PlayerController playerPC;				// PlayerController for instantiated player

	private bool spawnFlag = false;					// should we spawn the player?

	public InputController input;					// the input controller 
	// I think I need to modify how these scripts interact and follow the Inputcontroller model
	public UIController gameUI;						// the UIController object to manage the UI

	void Awake(){
		ActivateSingleton ();
	}

	void Start () {
		ResetScore ();

		input = gameObject.GetComponent<InputController> ();

		if (input == null)
			Debug.LogError ("Error: There is no InputController component on the GameController GameObject.");
		
		gameUI.TheGame = gameObject.GetComponent<GameController> ();
	}

	void Update () {
		if (spawnFlag) {
			player = 
				Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
			playerPC = player.GetComponent<PlayerController> ();
			input.SetPlayerController (playerPC);
			spawnFlag = false;
		}
	}

	void ResetScore(){
		score = 0;
	}

	public bool PlayerInstantiated(){
		return (player != null);
	}

	public void SetSpawnFlag(){
		if (!spawnFlag)
			spawnFlag = true;
	}

	// this is a little hacky, should probably be handled differently
	public void KillPlayer(){
		if (player != null) {
			player.GetComponent<PlayerController> ().Kill ();
		}
	}
}
