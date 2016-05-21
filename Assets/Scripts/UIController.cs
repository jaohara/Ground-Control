using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	private GameController theGame;						// to access score. ugly variable name
	public GameController TheGame {
		set { theGame = value; }
	}

	public Text score;									// score text object
	public Text health;									// health text object
	public Text shield;									// shield power text object
	public Text lives;									// lives text object
	public Text bombs;									// bombs text object

	// Use this for initialization
	void Start () {
		// this should be changed if the game is paused
		//Cursor.visible = false;
		UpdateScore ();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateScore ();
		UpdateHealth ();
		UpdateShield ();
		UpdateLives ();
		UpdateBombs ();
	}

	void UpdateScore(){
		if (theGame != null)
			score.text = theGame.Score.ToString();
	}


	/*
	 * There's... a lot of code reuse here. I wonder if I should condense these?
	 */
	void UpdateHealth(){
		if (theGame != null && theGame.player != null)
			health.text = theGame.playerPC.health.ToString ();
		else
			health.text = "---";
	}

	void UpdateShield(){
		if (theGame != null && theGame.player != null)
			shield.text = theGame.playerPC.playerShield.power.ToString ();
		else
			shield.text = "---";
	}

	void UpdateLives(){
		if (theGame != null && theGame.player != null)
			lives.text = theGame.playerPC.lives.ToString ();
		else
			lives.text = "-";
	}

	void UpdateBombs(){
		if (theGame != null && theGame.player != null)
			bombs.text = theGame.playerPC.playerBombs.bombs.ToString ();
		else
			bombs.text = "-";
	}
}
