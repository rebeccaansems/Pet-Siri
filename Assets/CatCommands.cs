 using UnityEngine;
using System.Collections;

public class CatCommands : MonoBehaviour {

    public GameObject cat;
    bool runAway = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.Get(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.Space))
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
        else if (command.Equals("Fetch"))
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 2);
            runAway = true;
            Vector3 rot = cat.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            cat.transform.rotation = Quaternion.Euler(rot);
        }
    }
}
