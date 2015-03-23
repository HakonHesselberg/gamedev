using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GUIText countText;
    public GUIText winText;
    public GUIText timerText;

    private bool timerStarted;
    private bool hasWon;
    private int count;
    private float timer;
    private float breakFactor;

    void Start()
    {
        count = 0;
        setCountText();
        setTimerText();
        winText.text = "";
        timer = 0f;
        timerStarted = false;
        hasWon = false;
        breakFactor = 1.5f;
    }

    void Update()
    {
        if (timerStarted && !hasWon)
        {
            timer += Time.deltaTime;
        }
        setTimerText();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = Vector3.zero;

        if (GetComponent<Rigidbody>().velocity.x * moveHorizontal > 0)
        {
            movement.x = 1f * Math.Sign(moveHorizontal);
        }
        else
        {
            movement.x = breakFactor * Math.Sign(moveHorizontal);
        }

        if (GetComponent<Rigidbody>().velocity.z * moveVertical > 0)
        {
            movement.z = 1f * Math.Sign(moveVertical);
        }
        else
        {
            movement.z = breakFactor * Math.Sign(moveVertical);
        }


        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero)
        {
            timerStarted = true;
        }

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.deltaTime);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count++;
            setCountText();
        }

        if (other.gameObject.tag == "Test")
        {
            other.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void setTimerText()
    {
        timerText.text = "Timer: " + Math.Round(timer, 2).ToString("F2");

        if (count >= 9)
        {
            hasWon = true;
            winText.text = "YOU WIN!  Your time was " + Math.Round(timer, 2).ToString("F2");
        }
    }

    private void setCountText()
    {
        countText.text = "Count: " + count + " out of 9";
        
    }
}
