using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

	public float speed;
	public float speedLimit = 8;
	public float speedUpValue = 0.5f;
	public static int bombPower;
	public int bombPowerLimit = 5;
	public static int bombAmount;
	public int bombAmountLimit = 5;
	public static bool live;

	void Start(){			
		live = true;
		speed = 4;
		bombPower = 1;
		bombAmount = 1;
	}

	void FixedUpdate(){	
		if (live == true) {
			Move ();
		} else {
			GetComponent<Rigidbody2D> ().constraints = 
			RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
			GetComponent<Animator> ().SetBool("Death", true);
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.CompareTag ("BombPowerUp"))
		{ // Increases blast range if current is less than threshold
			if (bombPower < bombPowerLimit) {		 
				bombPower++;
			}	Destroy (other.gameObject);
		}else if (other.gameObject.CompareTag ("BombAmountUp"))
		{ //Increases the amount of bombs, if the current one is less than the limit
			if (bombAmount < bombAmountLimit) {
				bombAmount++;
			}	Destroy (other.gameObject);
		}else if (other.gameObject.CompareTag ("SpeedUp"))
		{ //Increase character speed if current is less than threshold
			if (speed < speedLimit) {		
				speed += speedUpValue;
			}	Destroy (other.gameObject);
		}
	}

	void Move(){
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");	
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (h, v) * speed;

		GetComponent<Animator> ().SetInteger ("x", (int)h);
		GetComponent<Animator> ().SetInteger ("y", (int)v);
	}

	public void DestroyMe(){
		MenusManager.playerIsAlive = false;
		Destroy (gameObject);
	}

}
