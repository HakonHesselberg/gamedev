using System.Security.Policy;
using Assets.Scripts;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerBehaviour : MonoBehaviour, IAreaOfEffect
{
    public float health;

    // Use this for initialization
    void Start()
    {
        health = 100f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeAoEDamage(float damageAmount)
    {
        health -= damageAmount;
    }
}
