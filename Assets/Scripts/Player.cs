using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        Instance = this;
        _cam = Camera.main;
        _objProjectileManager = projectile.GetComponent<ProjectileManager>();
        _tr = transform;
        _crosshairList = new List<GameObject>();
    }

    public void RemoveCrosshair(int index)
    {
        Destroy(_crosshairList[index]);
        _crosshairList.RemoveAt(index);
    }
    public void AddCrosshair()
    {
        GameObject crosshair = Instantiate(prefabCrosshair);
        _crosshairList.Add(crosshair);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_crosshairList.Count > 0)
        {
            Vector3 camPos = _cam.transform.position;
            for (int i = 0; i < _crosshairList.Count; i++)
                _crosshairList[i].transform.position = (_objProjectileManager.projList[i].transform.position - (camPos)).normalized * 3f + camPos;    
        }
        if (currentPlanet)
        {
            Vector3 planetPos = currentPlanet.transform.position;
            transform.RotateAround(planetPos, _tr.right, Input.GetAxis("Vertical"));
            transform.RotateAround(planetPos, _tr.forward, Input.GetAxis("Horizontal"));
        }
    }

   
}
