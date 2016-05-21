using UnityEngine;
using System.Collections;

//needed to use the canvas UI stuff.
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IKillable, IDamageable, IHealable {

	#region SINGLETON PATTERN
	//make this class a singleton
	public static PlayerController instance = null;
	public static PlayerController Instance{
		get{ return instance; }
	}

	void ActivateSingleton(){
		if (instance) {
			DestroyImmediate (gameObject);
			return;
		}

		instance = this;
		DontDestroyOnLoad (gameObject);
	}
	#endregion

	public bool isPaused;					// bool to prevent pause menu actions from bleeding into player controller input

	private float speed;					// current moving speed

	public int lives = 3;					// number of player lives
	public int health;						// player's current health
	public int maxHealth;					// player's health max

	//shield variables
	public GameObject shieldObject;			// physical representation of shield
	[HideInInspector]
	public ShieldController playerShield;	// the player's shield controller
	[HideInInspector]
	public bool shieldActive;				// is the shield up?

	public Vector3 shootDirection;			// A vector to store the direction of the right stick

	// GunController object attached to player
	[HideInInspector]
	public GunController playerGunController;

	// bombController
	public BombController playerBombs;

	public GameObject killPrefab;			// a prefab for the explosion
	private Rigidbody playerRb;				// player's Rigidbody
	private Transform playerTrans;			// player's transform		
	private bool airborne;					// boolean for whether or not player is airborne
	private bool groundColliderContact;		// is the player already touching another collider?
	private bool willJump;					// will a jump be queued this physics step?
	public float jumpPower;					// jump power
	public float jumpThreshold;				// length of ray to check if grounded and able to jump
	private bool fallOffCheck;				// will the vertical height be throttled due to jumpbuttonup?
	private float fallOffValue;				// amount to cut vertical velocity to on jumpbuttonup
	private float movementInputValue;		// value recorded from the horizontal axis this frame

	//public accessors for InputController
	public bool WillJump{get;set;}
	public float MovementInputValue{ get; set; }
	public bool Airborne{ get{return airborne;} }
	public bool FallOffCheck{ get; set; }

	private Material playerMaterial;		//this object's material
	private Color baseColor;				// this object's albedo

	private GameController theGame;
	private InputController input;
	private CameraShaker cameraShake;

	//audio sources
	//private AudioSource jumpSound;			// jumping sound effect
	//private AudioSource landSound;			// landing sound effect
	//private AudioSource walkSound;			// walking sound effect
	[HideInInspector]
	public AudioSource spawnSound;
	[HideInInspector]
	public AudioSource damageSound;
	[HideInInspector]
	public AudioSource shotSound;
	[HideInInspector]
	public AudioSource deathSound;
	[HideInInspector]
	public AudioSource shieldSound;
	[HideInInspector]
	public AudioSource bombSound;

	void Awake(){
		ActivateSingleton ();
	}

	void Start () {
		playerRb = GetComponent<Rigidbody> ();
		playerTrans = GetComponent<Transform> ();
		// this is tied to the order that I add the audiosources to the gameobject, might need retooling
		//jumpSound = GetComponent<AudioSource> ();
		//landSound = GetComponents<AudioSource> () [1];
		//walkSound = GetComponents<AudioSource> () [2];

		speed = .75f;
		jumpPower = 1050.0f;
		jumpThreshold = 0.75f;
		fallOffValue = 5.0f;
		groundColliderContact = false;
		shootDirection = Vector3.left;

		health = 100;
		maxHealth = 100;

		playerShield = gameObject.GetComponent<ShieldController> ();
		playerShield.player = gameObject.GetComponent<PlayerController> ();

		playerMaterial = gameObject.GetComponent<MeshRenderer> ().material;
		baseColor = playerMaterial.color;

		playerGunController = gameObject.GetComponent<GunController> ();
		playerBombs = gameObject.GetComponent<BombController> ();

		cameraShake = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraShaker> ();

		theGame = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		if (theGame == null)
			Debug.LogError ("PlayerController error: Couldn't find Game Controller!");

		input = theGame.gameObject.GetComponent<InputController> ();
		input.SetGunController (playerGunController);
		input.SetShieldController (playerShield);
		input.SetBombController (playerBombs);

		GameObject audioObject = GameObject.FindGameObjectWithTag ("AudioController");
		if (audioObject != null) {
			Debug.Log ("Found the audioObject!");
			AudioSource[] audioSources = audioObject.GetComponents<AudioSource> ();
			spawnSound = audioSources [0];
			damageSound = audioSources [1];
			shotSound = audioSources [2];
			deathSound = audioSources [3];
			shieldSound = audioSources [4];
			bombSound = audioSources [5];
			spawnSound.Play ();
		}
	}

	void Update (){
		if (!isPaused) {
			IsAirborne ();

			shootDirection = playerGunController.shootDirection;
		}
	}

	void FixedUpdate () {
		Move ();
		Jump ();

		//IsMoving ();					// this is here for when I have the movement sound
	}
		
	void Move () {
		if (MovementInputValue != 0) {
			Vector3 moveVector = new Vector3 (MovementInputValue * speed/2, 0, 0);
			if (moveVector != Vector3.zero)
				playerRb.MovePosition (playerRb.position + moveVector);
		}
	}

	void Jump (){
		if (WillJump) {
			playerRb.AddForce (new Vector3 (movementInputValue * speed, jumpPower, 0));
			//jumpSound.Play ();
			WillJump = false;
		}
		if (FallOffCheck) {
			//set vertical velocity to whichever is less, .5 or current velocity. second component of fallOffCheck
			float fallVelocity = Mathf.Min(fallOffValue, playerRb.velocity.y);
			playerRb.velocity = new Vector3 (playerRb.velocity.x, fallVelocity, playerRb.velocity.z);
			FallOffCheck = false;
		}
	}

	public void DisableShieldEffect(){
		shieldObject.SetActive (false);
	}
		
	public void ModifyScore(int scoreDifferential){
		theGame.Score = scoreDifferential;
	}
		
	void OnCollisionEnter(Collision col){
		// this older code is for sound and only for contact with an environment collider
		if (!groundColliderContact && !airborne) {
			//landSound.Play ();
			groundColliderContact = true;
		}
	}

	void OnCollisionExit(){
		groundColliderContact = false;
	}

	void IsAirborne (){
		Debug.DrawRay (playerTrans.position, Vector3.down * jumpThreshold);
		Ray jumpRay = new Ray (playerTrans.position, Vector3.down);
		airborne = !(Physics.Raycast (jumpRay, jumpThreshold));
	}


	//to be reused when sound is implemented
	/*
	void IsMoving (){
		if (Mathf.Abs(playerRb.velocity.x) > 0 && !airborne)
			walkSound.Play();
		else 
			walkSound.Pause();
	}
	*/

	// this should probably be part of the damageable interface or something
	void ResetColor(){
		playerMaterial.color = baseColor;
	}

	//kinda hacky but it's easier to get this working in here. to be called by shieldcontroller when heal() is called on it
	public void ShieldHealColor(){
		playerMaterial.color = Color.blue;
		Invoke ("ResetColor", 0.05f);
	}

	public void GainLife(int livesAdded){
		lives += livesAdded;
	}

	public void Heal(int damageHealed){
		health = (health + damageHealed) > maxHealth ? maxHealth : health + damageHealed;

		playerMaterial.color = Color.green;
		Invoke ("ResetColor", 0.05f);
	}

	public void Damage(int damageTaken){
		if (shieldActive) {
			playerShield.Damage(damageTaken);
		} else {
			damageSound.Play ();
			health -= damageTaken;
			if (health <= 0)
				Kill ();
			cameraShake.Shake ();
			playerMaterial.color = Color.white;
			Invoke ("ResetColor", 0.05f);
		}
	}
		
	public void Kill(){
		// the lives thing will need to reference something in the gameController
		//lives--;
		deathSound.Play();
		Destroy (Instantiate (killPrefab, playerRb.position, playerRb.rotation), 1.25f);
		Destroy (gameObject);
	}
}


/*
 * Graveyard of deprecated input methods - RIP
 */

/*
void MoveCheck(){
	movementInputValue = Input.GetAxis ("Horizontal");
}

void JumpCheck(){
	if (Input.GetButtonDown ("Jump") && !airborne)
		willJump = true;

	if (Input.GetButtonUp ("Jump") && airborne)
		fallOffCheck = true;
}

void ShieldCheck(){
	if (Input.GetButton ("Shield") && playerShield.power > 0) {
		shieldActive = true;
		Debug.Log ("Shield Held");
		shieldObject.SetActive (true);
	} else {
		shieldActive = false;
		Invoke ("DisableShieldEffect", shieldObject.GetComponent<ParticleSystem> ().duration);
	}
}
*/