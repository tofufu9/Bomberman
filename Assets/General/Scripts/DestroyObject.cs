using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

    public void DestroyMe()
    { //Used in the animation event at the end of the animation
        Destroy(gameObject);
    }
}
