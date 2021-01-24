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
    private ProjectileManager _objProjectileManager;
    private Camera _cam;
    private Transform _tr;

    private void Awake()
    {
        Instance = this;
        _cam = Camera.main;
        _objProjectileManager = projectile.GetComponent<ProjectileManager>();
        _tr = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_objProjectileManager.projList.Count > 0)
        {
            point.SetActive(true);
            Vector3 camPos = _cam.transform.position;
            point.transform.position = (_objProjectileManager.projList[_objProjectileManager.projList.Count - 1].transform.position - (camPos)).normalized * 2f + camPos;    
        }
        else
            point.SetActive(false);
        if (currentPlanet)
        {
            Vector3 planetPos = currentPlanet.transform.position;
            transform.RotateAround(planetPos, _tr.right, Input.GetAxis("Vertical"));
            transform.RotateAround(planetPos, _tr.forward, Input.GetAxis("Horizontal"));
        }
    }

   
}
