using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public UnityEngine.EventSystems.EventSystem gameEventSystem;

	public Camera mainCamera;
	private Animator mainCameraAnimator;

	void Awake(){
		ActivateSingleton ();
		input = gameObject.GetComponent<InputController> ();
		mainCameraAnimator = mainCamera.GetComponent<Animator> ();
		gameEventSystem = 
			GameObject.FindGameObjectWithTag ("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem> ();

		// I don't think this will work because the pausemenudefault is going to be inactive, so it can't be found
		// Attach manually via inspector
		if (gameUI.PauseMenuDefault == null)
			gameUI.PauseMenuDefault = GameObject.FindGameObjectWithTag ("PauseMenuDefault");

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
		if (isPaused) {
			
		} else if (spawnFlag) {
			player = 
				Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
			input.SetPlayerController (PlayerController.Instance);
			spawnFlag = false;
			lives--;
		}

		Debug.Log ("IsPaused: " + isPaused);
		Debug.Log ("Time.timeScale: " + Time.timeScale);
	}

	public void ResetGameState(){
		Destroy (player);

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject enemy in enemies) 
			Destroy (enemy);

		GameObject[] projectiles = GameObject.FindGameObjectsWithTag ("Projectile");
		foreach (GameObject projectile in projectiles)
			Destroy (projectile);

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
		gameUI.TitleMenuAnimator.SetBool ("MenuActive", false);
	}

	// couldn't I just make this a "flip all bools" function and remove the above?
	public void GameToMenuTransition(){
		mainCameraAnimator.SetBool ("GameCamera", false);
		inGame = false;
		inMenus = true;
		gameUI.GameUIAnimator.SetBool ("GameActive", false);

		//will I need to not do this so we start at the title screen and not the menu?
		//gameUI.TitleMenuAnimator.SetBool ("MenuActive", true);
		gameEventSystem.SetSelectedGameObject (gameUI.TitleMenuDefault);
	}

	public void PauseGame(){
		PauseFlip ();
		gameEventSystem.SetSelectedGameObject (gameUI.PauseMenuDefault);
		PauseTime ();

		// I was using the code below to account for the game menu animation time, but I removed it
		// because it was inconsistently working. 
		/*
		if (!isPaused)
			Invoke ("PauseTime", 1.0f / 12.0f);
		else
			PauseTime ();
		*/
	}

	void PauseTime(){
		Time.timeScale = Mathf.Abs(Time.timeScale - 1);
	}

	void PauseFlip(){
		//flips all pause booleans
		isPaused = !isPaused;
		mainCameraAnimator.SetBool ("IsPaused", !mainCameraAnimator.GetBool ("IsPaused"));
		gameUI.PauseMenu.SetActive (!gameUI.PauseMenu.activeSelf);
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

	public void QuitToMenu(){
	
	}

	public void QuitGame(){
		Application.Quit ();
	}

	public void ButtonTest(){
		Debug.Log("Button input read");
	}
}
