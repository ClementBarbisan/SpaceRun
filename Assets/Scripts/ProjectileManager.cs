﻿using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private bool onReturn = false;
    private bool addForce = false;
    private PlayerInputLite inputs;

    // Start is called before the first frame update
    void Start()
    {
        inputs = PlayerInputLite.Instance;
        // tag = "weapon";
        // _rb = GetComponent<Rigidbody>();
        _tr = transform;
        projList = new List<GameObject>();
        if (inputs == null)
            return;
        // _currentParent = _tr.parent;
        inputs.CreateAction("return", PlayerInputLite.Button.GripPressed, PlayerInputLite.TypeHand.LeftHand, PlayerInputLite.TypeController.XRController, InputActionType.Button, PlayerInputLite.InteractionType.PressOnly).performed += OnReturn;
        inputs.CreateAction("stopreturn", PlayerInputLite.Button.GripPressed, PlayerInputLite.TypeHand.LeftHand, PlayerInputLite.TypeController.XRController, InputActionType.Button, PlayerInputLite.InteractionType.ReleaseOnly).performed += OnStopReturn;
        inputs.CreateAction("fire", PlayerInputLite.Button.TriggerPressed, PlayerInputLite.TypeHand.LeftHand, PlayerInputLite.TypeController.XRController, InputActionType.Button,PlayerInputLite.InteractionType.PressAndRelease).performed += OnFire;
        inputs.CreateAction("addforce", PlayerInputLite.Button.TriggerPressed, PlayerInputLite.TypeHand.LeftHand, PlayerInputLite.TypeController.XRController, InputActionType.Button, PlayerInputLite.InteractionType.PressOnly).started += OnAddForce;
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
        // getProj = false;
        // tag = "projectile";
        // _rb.AddForce(transform.parent.forward * _force, ForceMode.VelocityChange);
        // _tr.parent = null;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        addForce = false;
        if (projList.Count < maxPrj)
        {
            if (IsServer)
                LaunchProjectile(Player.Instance.id, _tr.parent.position, _tr.parent.rotation);
            else
            {
                InvokeServerRpc("LaunchProjectile", Player.Instance.id, _tr.parent.position, _tr.parent.rotation);
            }

            
        }
        force = 1f;
    }

    public void OnStopReturn(InputAction.CallbackContext context)
    {
        onReturn = false;
        if (projList.Count > 0 && Player.Instance.indexProj < projList.Count)
        {
            projList[Player.Instance.indexProj].GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    
    public void OnReturn(InputAction.CallbackContext context)
    {
        onReturn = true;
        
    }

    public void OnAddForce(InputAction.CallbackContext context)
    {
        addForce = true;
    }
    
    // Update is called once per frame
    void Update()
    {
       if (projList.Count < maxPrj && addForce)
           force += Time.deltaTime * 75f;
       if (Player.Instance.indexProj < projList.Count && onReturn)
       {
           projList[Player.Instance.indexProj].GetComponent<Rigidbody>().isKinematic = true;
           projList[Player.Instance.indexProj].transform.position +=
               (Player.Instance.transform.position - projList[Player.Instance.indexProj].transform.position)
               .normalized *
               Time.deltaTime * 2f *
               ((Player.Instance.transform.position - projList[Player.Instance.indexProj].transform.position)
                   .magnitude + 5f);
       }
    }

    public void ChangeIndex()
    {
        for (int i = 0; i < projList.Count; i++)
            projList[i].GetComponent<Rigidbody>().isKinematic = false;
    }
}
