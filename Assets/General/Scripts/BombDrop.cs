using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDrop : MonoBehaviour {

	public GameObject bombPrefab;
	GameObject[] bombs;
	private bool canDrop = true;

	void Update ()
	{ //Checks if the current amount of bombs in the scenario does not exceed the player's bomb limit
		bombs = GameObject.FindGameObjectsWithTag("Bomba");
		if (bombs.Length < PlayerBehaviour.bombAmount && canDrop) {
			bombDrop ();
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{ // Checks when the Player is on top of the bomb
		if (other.CompareTag ("Bomba") || other.CompareTag("BossBomb")) {
			canDrop = false;
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{ //Checks when the Player leaves the bomb
		if (other.CompareTag ("Bomba")) {
			canDrop = true;
		}
	}

	void bombDrop(){
		if (Input.GetKeyDown ("space") && PlayerBehaviour.live == true) {
			Vector2 pos = transform.position;
			pos.x = (Mathf.Round (pos.x));
			pos.y = (Mathf.Round (pos.y));
			Instantiate (bombPrefab, pos, Quaternion.identity);
		}
	}

}


