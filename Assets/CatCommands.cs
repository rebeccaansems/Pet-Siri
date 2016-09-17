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
            cat.transform.Translate(Vector3.forward * Time.deltaTime * 10);

            if (cat.transform.position.z > 100)
            {
                Vector3 rot = cat.transform.rotation.eulerAngles;
                rot = new Vector3(rot.x, rot.y + 180, rot.z);
                cat.transform.rotation = Quaternion.Euler(rot);
                catTurned = true;
                FoodExplosion();
            }

            if (cat.transform.position.z < 0 && catTurned)
            {
                runAway = false;
                cat.GetComponent<Animator>().SetInteger("CatCommands", 0);
                catTurned = false;
            }
        }

        //reset animation
        if (cat.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 0);
        }
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
            Vector3 rot = cat.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            cat.transform.rotation = Quaternion.Euler(rot);
        }
    }

    public GameObject[] food;

    void FoodExplosion()
    {
        int foodItem = Random.Range(0, 4);
        for(int i=0; i< Random.Range(350, 500); i++)
        {
            Instantiate(food[foodItem], new Vector3(Random.Range(-15, 15), Random.Range(500, 650), Random.Range(-35, 15)), Quaternion.identity);
        }
    }
}
