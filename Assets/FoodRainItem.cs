using UnityEngine;
using System.Collections;

public class FoodRainItem : MonoBehaviour {

    int spin;

    void Start()
    {
        spin = Random.Range(0, 100);
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, spin * Time.deltaTime);
        if (transform.position.y < -1)
        {
            Destroy(this.gameObject);
        }
	}
}
