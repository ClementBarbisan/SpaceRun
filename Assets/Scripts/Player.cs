using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public GameObject currentPlanet = null;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlanet)
        {
            transform.RotateAround(currentPlanet.transform.position, transform.right, Input.GetAxis("Vertical"));
            transform.RotateAround(currentPlanet.transform.position, transform.forward, Input.GetAxis("Horizontal"));
        }
    }

    // private void OnCollisionStay(Collision other)
    // {
    //     transform.up = other.contacts[0].normal;
    // }
}
