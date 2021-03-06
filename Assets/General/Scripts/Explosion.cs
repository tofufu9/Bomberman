using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){				
		
		if (other.gameObject.CompareTag("Destroyable")||other.gameObject.CompareTag("Bomba")
			||other.gameObject.CompareTag("BombAmountUp")||other.gameObject.CompareTag("BombPowerUp")
			||other.gameObject.CompareTag("SpeedUp")||other.gameObject.CompareTag("Enemy")){
			Destroy (other.gameObject);
		}
		if (other.gameObject.CompareTag ("Player")) {
			PlayerBehaviour.live = false;
		}        
	}
}
