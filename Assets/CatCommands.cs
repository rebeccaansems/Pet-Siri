 using UnityEngine;
using System.Collections;

public class CatCommands : MonoBehaviour {

    public GameObject cat;
    bool runAway = false, catTurned = false;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Button.DpadUp) || Input.GetKey(KeyCode.W))
        {
            CommandHeard("Jump");
        }

        if (OVRInput.Get(OVRInput.Button.DpadLeft) || Input.GetKey(KeyCode.D))
        {
            CommandHeard("Fetch");
        }

        if (Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }

        if (runAway)
        {
            if (cat.transform.position.z > 200)
            {
                Vector3 rot = cat.transform.rotation.eulerAngles;
                rot = new Vector3(rot.x, rot.y + 180, rot.z);
                cat.transform.rotation = Quaternion.Euler(rot);
                FoodExplosion();
                catTurned = true;
            }
            
            if (cat.transform.position.z < 0 && catTurned)
            {
                runAway = false;
                cat.GetComponent<Animator>().SetInteger("CatCommands", 0);
                catTurned = false;
            }
            else if (cat.transform.position.z < 20 && catTurned)
            {
                cat.GetComponent<Animator>().SetInteger("CatCommands", 2);
            }
            else if (cat.transform.position.z < 0)
            {
                cat.transform.rotation = Quaternion.RotateTowards(cat.transform.rotation, _targetRotation, turningRate * Time.deltaTime);
            }

            if ((cat.transform.rotation.y == 0 || cat.transform.rotation.y == 360) && !catTurned)
            {
                cat.GetComponent<Animator>().SetInteger("CatCommands", 3);
                cat.transform.Translate(Vector3.forward * Time.deltaTime * 20);
            }
            else if (catTurned)
            {
                if(cat.GetComponent<Animator>().GetInteger("CatCommands") == 2)
                {
                    cat.transform.Translate(Vector3.forward * Time.deltaTime * 5);
                }
                else
                {
                    cat.transform.Translate(Vector3.forward * Time.deltaTime * 20);
                }
            }
        }

        //reset animation
        if (cat.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 0);
        }
    }

    // Maximum turn rate in degrees per second.
    public float turningRate = 30f;
    // Rotation we should blend towards.
    private Quaternion _targetRotation = Quaternion.Euler(0, 0, 0);
    // Call this when you want to turn the object smoothly.
    public void SetBlendedEulerAngles(Vector3 angles)
    {
        _targetRotation = Quaternion.Euler(angles);
    }

    void CommandHeard(string command)
    {
        if (command.Equals("Jump"))
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 1);
        }
        else if (command.Equals("Meow"))
        {
        }
        else if (command.Equals("Fetch") && !runAway)
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 2);
            runAway = true;
            catTurned = false;
        }
    }

    public GameObject[] food;

    void FoodExplosion()
    {
        int foodItem = Random.Range(0, 4);
        if (!catTurned)
        {
            for (int i = 0; i < Random.Range(750, 1000); i++)
            {
                Instantiate(food[foodItem], new Vector3(Random.Range(-50, 50), Random.Range(750, 1000), Random.Range(-65, 50)), Quaternion.identity);
            }
        }
    }
}
