using System;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using UnityEngine;
using System.Collections;

public class PlayerLogic : MonoBehaviour
{
    public Material DefaultCheckpointMaterial;
    public Material CurrentCheckpointMaterial;
    public GUISkin GUITimerSkin;

    private Vector3 respawnPosition;
    private int numberOfDeaths = 0;
    private float timer;
    private bool hasWon;

	// Use this for initialization
	void Start ()
	{
	    respawnPosition = transform.position;
	    timer = Time.time;
	}

    void Update ()
    {

        if (transform.position.y < -10f)
        {
            Death();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Death();
        }
    }

    void OnGUI ()
    {
        GUI.skin = GUITimerSkin;

        if (hasWon)
        {
            GUI.Label(new Rect(20, 20, 300, 300), "Final time: " + Math.Round(timer, 2).ToString("F2"));            
        }
        else
        {
            GUI.Label(new Rect(20, 20, 300, 300), "Time: " + Math.Round(Time.timeSinceLevelLoad, 2).ToString("F2"));            
        }
        
        GUI.Label(new Rect(20, 40, 300, 300), "Deaths: " + numberOfDeaths);
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            foreach (var checkpoint in GameObject.FindGameObjectsWithTag("Checkpoint"))
            {
                checkpoint.renderer.material = DefaultCheckpointMaterial;
            }

            respawnPosition = other.transform.position;
            other.renderer.material = CurrentCheckpointMaterial;
        }

        if (other.tag == "Goal")
        {
            if (!hasWon)
            {
                timer = Time.time - timer;
            }
            hasWon = true;
        }
    }

    void Death ()
    {
        transform.position = respawnPosition;
        transform.rigidbody.velocity = Vector3.zero;
        if (!hasWon)
        {
            numberOfDeaths++;
        }
    }
}
