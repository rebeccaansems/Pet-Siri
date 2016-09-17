using UnityEngine;
using System.Collections;

public class FoodRainItem : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
	    if(transform.position.y < -1)
        {
            Destroy(this.gameObject);
        }
	}
}
