using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Random.seed = (int)Time.time;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_crosshairList.Count > 0)
        {
            Vector3 camPos = _cam.transform.position;
            for (int i = 0; i < _crosshairList.Count; i++)
            {
                if (i == indexProj)
                    _crosshairList[i].transform.localScale = currentScaleCrosshair * 2.5f;
                else
                    _crosshairList[i].transform.localScale = currentScaleCrosshair;
                _crosshairList[i].transform.position =
                    (_objProjectileManager.projList[i].transform.position - (camPos)).normalized * 3f + camPos;
            }
        }

        timeOut -= Time.deltaTime;
        if (Mathf.Abs(Input.GetAxis("HorizontalLeft")) > 0.05f && timeOut <= 0 && _crosshairList.Count > 1)
        {
            indexProj = Mathf.Clamp(0, _crosshairList.Count - 1, indexProj + (int)Mathf.Sign(Input.GetAxis("HorizontalLeft")) * 1 % _crosshairList.Count);
            _objProjectileManager.ChangeIndex();
            timeOut = 0.25f;
        }
        if (currentPlanet)
        {
            Vector3 planetPos = currentPlanet.transform.position;
            transform.RotateAround(planetPos, _tr.right, Input.GetAxis("Vertical"));
            transform.RotateAround(planetPos, _tr.forward, Input.GetAxis("Horizontal"));
        }
    }

   
}
