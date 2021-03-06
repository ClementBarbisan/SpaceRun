﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public static Player Instance;
    private List<GameObject> _crosshairList;
    [SerializeField]
    private GameObject prefabCrosshair;
    public GameObject currentPlanet = null;
    // [SerializeField] private GameObject point;
    [SerializeField] private GameObject projectile;
    private ProjectileManager _objProjectileManager;
    private Camera _cam;
    private Transform _tr;
    public int indexProj = 0;
    private Vector3 currentScaleCrosshair = Vector3.one;
    [SerializeField] private float timeOut;
    public ulong id = 0;
    public int kills = 0;
    public int dead = 0;
    private PlayerInputLite inputs;


    private void Awake()
    {
        Instance = this;
        currentScaleCrosshair = prefabCrosshair.transform.localScale;
        _cam = Camera.main;
        _objProjectileManager = projectile.GetComponent<ProjectileManager>();
        _tr = transform;
        _crosshairList = new List<GameObject>();
    }

    
    
    public void RemoveCrosshair(int index)
    {
        Destroy(_crosshairList[index]);
        _crosshairList.RemoveAt(index);
        if (indexProj >= _crosshairList.Count)
        {
            indexProj = Mathf.Clamp(0, _crosshairList.Count - 1, indexProj--);
            _objProjectileManager.ChangeIndex();
        }

    }

    public void StartPosition()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        Random.InitState((int)Time.time);
        int index = Random.Range(0, planets.Length);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(_tr.position, planets[index].gameObject.transform.position - _tr.position, out hit);
        _tr.position = hit.point;
        _tr.up = hit.normal;
        _tr.parent = hit.collider.gameObject.transform;
    }
    
    public void AddCrosshair()
    {
        GameObject crosshair = Instantiate(prefabCrosshair);
        _crosshairList.Add(crosshair);
    }
    // Start is called before the first frame update
    void Start()
    {
        inputs = PlayerInputLite.Instance;
        if (inputs == null)
            return;
        inputs.CreateAction("walk", PlayerInputLite.Vector2.Joystick, PlayerInputLite.TypeHand.RightHand,
            PlayerInputLite.TypeController.XRController, InputActionType.Value,
            PlayerInputLite.InteractionType.PressOnly).started += OnWalk;
        inputs.CreateAction("selectProjectile", PlayerInputLite.Vector2.Joystick, PlayerInputLite.TypeHand.LeftHand, 
            PlayerInputLite.TypeController.XRController, InputActionType.Value,
            PlayerInputLite.InteractionType.PressOnly).started += OnSelectProjectile;
    }
    
    public void OnSelectProjectile(InputAction.CallbackContext context)
    {
        if (Mathf.Abs(context.ReadValue<Vector2>().x) > 0.05f && timeOut <= 0 && _crosshairList.Count > 1)
        {
            indexProj = Mathf.Clamp(indexProj + (int)Mathf.Sign(context.ReadValue<Vector2>().x) * 1 % _crosshairList.Count, 0, _crosshairList.Count - 1);
            _objProjectileManager.ChangeIndex();
            timeOut = 0.25f;
        }
    }
    
    public void OnWalk(InputAction.CallbackContext context)
    {
        if (currentPlanet)
        {
            Vector3 planetPos = currentPlanet.transform.position;
            transform.RotateAround(planetPos, _tr.right, context.ReadValue<Vector2>().y);
            transform.RotateAround(planetPos, _tr.forward, context.ReadValue<Vector2>().x);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_crosshairList.Count > 0)
        {
            Vector3 camPos = _cam.gameObject.transform.position;
            for (int i = 0; i < _crosshairList.Count; i++)
            {
                if (i == indexProj)
                    _crosshairList[i].transform.localScale = currentScaleCrosshair * 2.5f;
                else
                    _crosshairList[i].transform.localScale = currentScaleCrosshair;
                _crosshairList[i].transform.position =
                    (_objProjectileManager.projList[i].transform.position - (camPos)).normalized * 3f + camPos;
                _crosshairList[i].transform.forward =
                    (_objProjectileManager.projList[i].transform.position - (camPos));
            }
        }

        timeOut -= Time.deltaTime;
       
       
    }

   
}
