using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	// Title Menu UI
	[Header("Title Menu UI")]
	public Animator TitleMenuAnimator;

	// in game UI 
	[Header("In-Game UI")]
	public Animator GameUIAnimator;
	public Text score;									// score text object
	public Text health;									// health text object
	public Text shield;									// shield power text object
	public Text lives;									// lives text object
	public Text bombs;									// bombs text object

	void Start () {
		// this should be true only if game is paused, but false for testing now
		//Cursor.visible = false;
		UpdateScore ();
	}

	void Update () {
		UpdateScore ();
		UpdateHealth ();
		UpdateShield ();
		UpdateLives ();
		UpdateBombs ();
	}

	void UpdateScore(){
		if (GameController.Instance != null)
			score.text = GameController.Instance.Score.ToString();
	}
		
	bool GameAndPlayerExist(){
		return (GameController.Instance != null && PlayerController.Instance != null);
	}

	void UpdateHealth(){
		if (GameAndPlayerExist())
			health.text = PlayerController.Instance.health.ToString ();
		else
			health.text = "---";
	}

	void UpdateShield(){
		if (GameAndPlayerExist())
			shield.text = PlayerController.Instance.playerShield.power.ToString ();
		else
			shield.text = "---";
	}

	void UpdateLives(){
		if (GameController.Instance != null)
			lives.text = GameController.Instance.Lives.ToString ();
		else
			lives.text = "-";
	}

	void UpdateBombs(){
		if (GameController.Instance != null)
			bombs.text = GameController.Instance.Bombs.ToString ();
		else
			bombs.text = "-";
	}
}
