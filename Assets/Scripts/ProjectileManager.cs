﻿using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileManager : NetworkedBehaviour
{
    // private Rigidbody _rb;
    // public bool getProj = true;
    // private Transform _currentParent;
    [SerializeField] private GameObject prefabProj;
    [FormerlySerializedAs("_force")] public float force;
    private Transform _tr;
    [SerializeField]
    private int maxPrj = 3;
    public List<GameObject> projList;
    // Start is called before the first frame update
    void Awake()
    {
        // tag = "weapon";
        // _rb = GetComponent<Rigidbody>();
        _tr = transform;
        projList = new List<GameObject>();
        // _currentParent = _tr.parent;
    }

    public void GetProjectile(GameObject proj)
    {
        Player.Instance.RemoveCrosshair(projList.IndexOf(proj));
        projList.Remove(proj);
        Destroy(proj);
        
        // transform.parent = _currentParent;
        // getProj = true;
        // tag = "weapon";
        // _rb.velocity = Vector3.zero;
        // _rb.angularVelocity = Vector3.zero;
        // _tr.position = _tr.parent.position;
        // _tr.rotation = _tr.parent.rotation;
    }
    [ServerRPC(RequireOwnership = false)]
    void LaunchProjectile(ulong id, Vector3 pos, Quaternion rot)
    {
        GameObject go = Instantiate(prefabProj, pos, rot);
        go.GetComponent<NetworkedObject>().SpawnWithOwnership(id);
        force = 1f;
        // getProj = false;
        // tag = "projectile";
        // _rb.AddForce(transform.parent.forward * _force, ForceMode.VelocityChange);
        // _tr.parent = null;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("joystick button 14") && projList.Count < maxPrj)
            force += Time.deltaTime * 75f;
        if (Input.GetKeyUp("joystick button 14") && projList.Count < maxPrj)
        {
            if (isServer)
                LaunchProjectile(Player.Instance.id, _tr.parent.position, _tr.parent.rotation);
            else
            {
                InvokeServerRpc("LaunchProjectile", Player.Instance.id, _tr.parent.position, _tr.parent.rotation);
            }
        }

        if (Input.GetKey("joystick button 4") && Player.Instance.indexProj < projList.Count)
        {
            projList[Player.Instance.indexProj].GetComponent<Rigidbody>().isKinematic = true;
            projList[Player.Instance.indexProj].transform.position +=
                (Player.Instance.transform.position - projList[Player.Instance.indexProj].transform.position).normalized *
                Time.deltaTime * 2f * ((Player.Instance.transform.position - projList[Player.Instance.indexProj].transform.position).magnitude + 5f);
            // GetProjectile(projList[projList.Count - 1]);
        }
        else if (projList.Count > 0 && Player.Instance.indexProj < projList.Count)
        {
            projList[Player.Instance.indexProj].GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void ChangeIndex()
    {
        for (int i = 0; i < projList.Count; i++)
            projList[i].GetComponent<Rigidbody>().isKinematic = false;
    }
}