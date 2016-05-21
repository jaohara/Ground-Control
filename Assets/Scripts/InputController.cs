
using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour
{
	// movement axes
	private const string horizontalAxis = "Horizontal";

	// look axes
	private const string lookHorizontal = "RSHorizontal";
	private const string lookVertical = "RSVertical";

	// button strings
	private const string shootInput = "Fire1";
	private const string jumpInput = "Jump";
	private const string bombInput = "Bomb";
	private const string shieldInput = "Shield";
	private const string swapInput = "AimToggle";
	private const string selfDestructInput = "SelfDestruct";

	// sub-controllers 
	private GameController game;
	private PlayerController player;
	private GunController weapon;
	private ShieldController shield;
	private BombController bomb;

	public bool controllerLook = true;
	public bool isPaused;

	void Start (){
		game = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();

		if (game == null)
			Debug.LogError ("InputController cannot find GameController.");
	}

	void Update (){
		if (!isPaused) {
			if (player != null) {
				SelfDestructCheck ();
				SwapLookCheck ();
				LookCheck ();

				JumpCheck ();
				MoveCheck ();

				ShieldCheck ();
				BombCheck ();
				ShootCheck ();
			} else {
				SpawnCheck ();
			}
		}
	}

	// routes to PlayerController
	void JumpCheck(){
		if (Input.GetButtonDown (jumpInput) && !player.Airborne)
			player.WillJump = true;

		if (Input.GetButtonUp (jumpInput) && player.Airborne)
			player.FallOffCheck = true;	
	}

	// routes to PlayerController
	void MoveCheck(){
		player.MovementInputValue = Input.GetAxis (horizontalAxis);
	}

	// routes to PlayerController
	void ShieldCheck(){
		/*
		 * parts of this method should be rerouted to shieldcontroller instead of 
		 * using playercontroller for handling all of this. also, some of the shield functionality
		 * shouldn't be located here. This class is for handling input, not the implementation
		 * of the input.
		 */
		if (Input.GetButton (shieldInput) && player.playerShield.power > 0) {
			player.shieldActive = true;
			player.shieldObject.SetActive (true);
		} else {
			player.shieldActive = false;
			player.Invoke ("DisableShieldEffect", player.shieldObject.GetComponent<ParticleSystem> ().duration);
		}
	}

	// new
	void BombCheck(){
		if (Input.GetButtonDown (bombInput)) {
			Debug.Log ("Read BombInput in InputController.BombCheck()");
			bomb.TriggerExplosion ();
		}
	}

	// routes to GunController
	void LookCheck(){
		/*
		 * I've discovered an issue with the mouse controls here. The sensitivity is going to be increased 
		 * the closer the cursor gets to the center point in the screen.
		 * 
		 * Think of two concentric circles, an inner, smaller circle and an outer, larger circle. Because
		 * the circumference of the inner circle is less, the same amount of distance along the edge of each
		 * circle represents a larger percentage of the circle. A small movement might be 10% of the circumference
		 * when dealing with the outer circle but 33% on the inner circle.
		 * 
		 * I need to find a way to normalize the mouse movement so the amount that the cursor rotates is 
		 * agnostic of the distance from the center of the circle. Maybe I need to consider changing my method
		 * of detecting mouse input, and not just finding the delta of the mousex and y movements?
		 */

		if (weapon != null) {
			Vector3 frameShootDirection;

			if (controllerLook) {
				frameShootDirection = Vector3.right * Input.GetAxis (lookHorizontal) + Vector3.up * Input.GetAxis (lookVertical);
			} else {
				frameShootDirection = Input.mousePosition;
				frameShootDirection.x -= Screen.width / 2;
				frameShootDirection.y -= Screen.height / 2;
			}

			if (frameShootDirection.sqrMagnitude >= 0.1f)
				weapon.shootDirection = frameShootDirection;
		}
	}
		
	// routes to GunController. Mixing too much functionality here?
	void ShootCheck(){
		if (Input.GetAxis (shootInput) > 0.0)
			weapon.StartCoroutine ("Shoot");
	}

	// new, functionality from GameController
	void SpawnCheck(){
		// a little hacky, but spawn will check if jump or shoot are pressed.
		if ((Input.GetButtonDown (jumpInput) || Input.GetButtonDown (shootInput)) && !game.PlayerInstantiated ()) {
			// we can only be in here if the player isn't instantiated
			game.SetSpawnFlag();
		}
	}

	// from guncontroller. Useless?
	void SwapLookCheck(){
		if (Input.GetButtonDown(swapInput))
			SwapLookControl();
	}

	public void SwapLookControl(){
		controllerLook = !controllerLook;
	}
		
	// this is just for debugging purposes
	void SelfDestructCheck(){
		/*
		if (Input.GetButtonDown (selfDestructInput)) {
			game.KillPlayer ();
		}
		*/
		//repurposed to turn on AI
		if (Input.GetButtonDown (selfDestructInput)) {
			GameObject AIGO = GameObject.FindGameObjectWithTag ("AI");
			AIGO.GetComponent<EnemySpawnerIdea> ().canSpawn = !AIGO.GetComponent<EnemySpawnerIdea> ().canSpawn;
		}
	}

	// set the controllers
	public void SetPlayerController(PlayerController pc){
		if (pc != null)
			player = pc;
	}

	public void SetGunController(GunController gc){
		if (gc != null)
			weapon = gc;
	}

	public void SetShieldController(ShieldController sc){
		if (sc != null)
			shield = sc;
	}

	public void SetBombController(BombController bc){
		if (bc != null)
			bomb = bc;
	}
}

