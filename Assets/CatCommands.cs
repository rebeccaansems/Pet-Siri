using UnityEngine;
using System.Collections;
using System;
using UnitySpeechToText.Services;

public class CatCommands : MonoBehaviour
{

    public GameObject cat, kitten;
    bool runAway = false, catTurned = false, getFriend = false, catHidden = false;
    public WatsonStreamingSpeechToTextService m_SpeechToTextService;

    // Use this for initialization
    void Start()
    {
        m_SpeechToTextService.RegisterOnError(OnError);
        m_SpeechToTextService.RegisterOnTextResult(OnTextResult);
        m_SpeechToTextService.RegisterOnRecordingTimeout(OnRecordingTimeout);
        m_SpeechToTextService.StartRecording();
    }

    // Update is called once per frame
    void Update()
    {

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
                if (getFriend)
                {
                    catHidden = true;
                }
                else 
                {
                    FoodExplosion();
                }
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
                if (cat.GetComponent<Animator>().GetInteger("CatCommands") == 2)
                {
                    cat.transform.Translate(Vector3.forward * Time.deltaTime * 5);
                }
                else
                {
                    cat.transform.Translate(Vector3.forward * Time.deltaTime * 20);
                }
            }
        }

        if (getFriend && catHidden)
        {
            KittenFriendRun();
        }

        //reset animation
        if (cat.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jump")
            || cat.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("IdleSit"))
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 0);
        }
    }

    void KittenFriendRun()
    {
        kitten.GetComponent<Animator>().SetInteger("CatCommands", 3);
        kitten.transform.Translate(Vector3.forward * Time.deltaTime * 20);
        if(kitten.transform.position.z < -100)
        {
            catHidden = false;
            kitten.transform.position = new Vector3(-1, -0.62f, 220);
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
        if (command.Contains("jump") || command.Contains("john"))//jump
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 1);
        }
        else if (command.Contains("sit"))
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 4);
        }
        else if ((command.Contains("away")) && !runAway)//fetch
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 2);
            runAway = true;
            catTurned = false;
            getFriend = false;
        }
        else if ((command.Contains("friend")|| command.Contains("brenda")) && !runAway)//fetch
        {
            cat.GetComponent<Animator>().SetInteger("CatCommands", 2);
            runAway = true;
            catTurned = false;
            getFriend = true;
        }
    }

    public GameObject[] food;

    void FoodExplosion()
    {
        int foodItem = UnityEngine.Random.Range(0, 4);
        if (!catTurned)
        {
            for (int i = 0; i < UnityEngine.Random.Range(750, 1000); i++)
            {
                Instantiate(food[foodItem], new Vector3(UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(750, 1000), UnityEngine.Random.Range(-65, 50)), Quaternion.identity);
            }
        }
    }

    void OnError(string text)
    {
        Debug.LogError(text);
    }

    // Note that handling interim results is only necessary if your speech-to-text service is streaming.
    // Non-streaming speech-to-text services should only return one result per recording.
    void OnTextResult(SpeechToTextResult result)
    {
        for (int i = 0; i < result.TextAlternatives.Length; ++i)
        {
            Debug.Log(result.TextAlternatives[i].Text);
            CommandHeard(result.TextAlternatives[i].Text);
        }
    }

    void OnRecordingTimeout()
    {
        Debug.Log("Timeout");
    }
}
