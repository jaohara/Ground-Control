using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	#region SINGLETON PATTERN
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

	private int score;
	public int Score {
		get{ return score; }
		set{ score += value; }
	}

	public int defaultLives = 3;
	private int lives;
	public int Lives{
		get{ return lives; }
		set{ lives += value; }
	}

	public int defaultBombs = 3;
	private int bombs;
	public int Bombs{
		get { return bombs; }
		set { bombs += value; }
	}

	private bool inMenus = true;
	public bool InMenus{
		get { return inMenus; }
	}

	private bool inGame = false;
	public bool InGame{
		get { return inGame; }
	}

	private bool isPaused = false;
	public bool IsPaused{
		get { return isPaused; }
	}

	public GameObject playerPrefab;					// prefab of the player to be spawned.
	public Transform spawnPoint;					// trans of the spawn point.
	[HideInInspector]
	public GameObject player;						// instantiated player

	private bool spawnFlag = false;					// should we spawn the player?

	public InputController input;					// the input controller 
	// I think I need to modify how these scripts interact and follow the Inputcontroller model
	public UIController gameUI;						// the UIController object to manage the UI

	public Camera mainCamera;
	private Animator mainCameraAnimator;

	void Awake(){
		ActivateSingleton ();
		input = gameObject.GetComponent<InputController> ();
		mainCameraAnimator = mainCamera.GetComponent<Animator> ();

		if (input == null)
			Debug.LogError ("Error: There is no InputController component on the GameController GameObject.");

		if (mainCameraAnimator == null)
			Debug.LogError ("Error: There is no Animator on the Main Camera.");
	}

	void Start () {
		// probably shouldn't be called yet
		ResetGameState ();
	}

	void Update () {
		if (spawnFlag) {
			player = 
				Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
			input.SetPlayerController (PlayerController.Instance);
			spawnFlag = false;
			lives--;
		}
	}

	void ResetGameState(){
		ResetScore ();
		lives = defaultLives;
		bombs = defaultBombs;
	}

	void ResetScore(){
		score = 0;
	}

	public void MenuToGameTransition(){
		mainCameraAnimator.SetBool ("GameCamera", true);
		inGame = true;
		inMenus = false;
		gameUI.GameUIAnimator.SetBool ("GameActive", true);
	}

	public void PauseFlip(){
		isPaused = !isPaused;
	}

	public bool PlayerInstantiated(){
		return (player != null);
	}

	public void SetSpawnFlag(){
		if (!spawnFlag && lives > 0) {
			spawnFlag = true;
		} 
	}

	// this is a little hacky, should probably be handled differently
	public void KillPlayer(){
		if (player != null) {
			player.GetComponent<PlayerController> ().Kill ();
		}
	}

	public void QuitGame(){
		Application.Quit ();
	}
}
