using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private GameObject wIndicator;
    private GameObject aIndicator;
    private GameObject sIndicator;
    private GameObject dIndicator;

    private Vector3 powerIndicatorSmallestScale = new Vector3(0f, 0.51f, 0f);
    private Vector3 powerIndicatorLargestScale = new Vector3(0.7f, 0.51f, 0.7f);

    private const float explosionRadius = 5.0f;
    private const float minExplosionForce = 100.0f;
    private const float maxExplosionForce = 700.0f;
    private const float maxHoldTime = 1.0f;
    private const float keyDownMinTime = 0.2f;

    private float wKeyDown;
    private float aKeyDown;
    private float sKeyDown;
    private float dKeyDown;

    private float wKeyUp;
    private float aKeyUp;
    private float sKeyUp;
    private float dKeyUp;

    void Start()
    {
        wIndicator = GameObject.Find("wIndicator");
        aIndicator = GameObject.Find("aIndicator");
        sIndicator = GameObject.Find("sIndicator");
        dIndicator = GameObject.Find("dIndicator");

        wIndicator.transform.localScale = powerIndicatorSmallestScale;
        aIndicator.transform.localScale = powerIndicatorSmallestScale;
        sIndicator.transform.localScale = powerIndicatorSmallestScale;
        dIndicator.transform.localScale = powerIndicatorSmallestScale;
    }
	
	void Update ()
    {

        #region KeyDown
        if (Input.GetKeyDown(KeyCode.W))
        {
            wKeyDown = Time.time;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            aKeyDown = Time.time;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            sKeyDown = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            dKeyDown = Time.time;
        }
        #endregion

        #region KeyHeld
	    if (Input.GetKey(KeyCode.W))
	    {
	        ResizePowerIndicators(wIndicator, wKeyDown);
	    }

        if (Input.GetKey(KeyCode.A))
        {
            ResizePowerIndicators(aIndicator, aKeyDown);
        }

        if (Input.GetKey(KeyCode.S))
        {
            ResizePowerIndicators(sIndicator, sKeyDown);
        }

        if (Input.GetKey(KeyCode.D))
        {
            ResizePowerIndicators(dIndicator, dKeyDown);
        }
        #endregion

        #region KeyUp
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (Time.time - wKeyUp > keyDownMinTime)
            {
                wKeyUp = Time.time;
                DoExplosion(Time.time - wKeyDown, new Vector3(0f, -2f, 0f));
            }
            wIndicator.transform.localScale = powerIndicatorSmallestScale;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            if (Time.time - aKeyUp > keyDownMinTime)
            {
                aKeyUp = Time.time;
                DoExplosion(Time.time - aKeyDown, new Vector3(2f, 0f, 0f));
            }
            aIndicator.transform.localScale = powerIndicatorSmallestScale;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            if (Time.time - sKeyUp > keyDownMinTime)
            {
                sKeyUp = Time.time;
                DoExplosion(Time.time - sKeyDown, new Vector3(0f, 2f, 0f));
            }
            sIndicator.transform.localScale = powerIndicatorSmallestScale;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            if (Time.time - dKeyUp > keyDownMinTime)
            {
                dKeyUp = Time.time;
                DoExplosion(Time.time - dKeyDown, new Vector3(-2f, 0f, 0f));
            }
            dIndicator.transform.localScale = powerIndicatorSmallestScale;
        }
        #endregion

	}

    private void ResizePowerIndicators(GameObject powerIndicator, float keyDownTime)
    {
        float amountToResize = 0;
        float resizeRange = powerIndicatorLargestScale.x - powerIndicatorSmallestScale.x;
        float totalTimeRange = maxHoldTime - keyDownMinTime;
        float resizeIncrement = resizeRange/totalTimeRange;
        float timeHeld = Time.time - keyDownTime;
        if (timeHeld > maxHoldTime)
        {
            amountToResize = resizeIncrement*maxHoldTime;
        }
        else if (timeHeld < keyDownMinTime)
        {
            amountToResize = resizeIncrement*keyDownMinTime;
        }
        else
        {
            amountToResize = resizeIncrement*timeHeld;
        }

        powerIndicator.transform.localScale = new Vector3(amountToResize, powerIndicator.transform.localScale.y, amountToResize);
    }

    private void DoExplosion(float timeHeld, Vector3 explosionOffset)
    {
        Vector3 explosionPosition = transform.position + explosionOffset;

        if (timeHeld > maxHoldTime) timeHeld = maxHoldTime; 

        float explosionForce = (timeHeld/maxHoldTime)*maxExplosionForce;
        
        if (explosionForce < minExplosionForce) explosionForce = minExplosionForce;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit && hit.rigidbody)
            {
                hit.rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
        }
    }
}
