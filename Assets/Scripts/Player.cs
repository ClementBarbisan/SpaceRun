using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public GameObject currentPlanet = null;
    [SerializeField] private GameObject point;
    [SerializeField] private GameObject projectile;
    private Projectile objProjectile;
    private Camera cam;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
        objProjectile = projectile.GetComponent<Projectile>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!objProjectile.getProj)
        {
            point.SetActive(true);
            point.transform.position = (projectile.transform.position - (Camera.main.transform.position)).normalized * 2f + Camera.main.transform.position;    
        }
        else
            point.SetActive(false);
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
