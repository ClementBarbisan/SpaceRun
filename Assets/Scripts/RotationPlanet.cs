using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using Random = UnityEngine.Random;

public class RotationPlanet : NetworkedBehaviour
{
    public float speed = 1;
    private Vector3 _initialPos;
    private Transform _tr;

    private Vector3 _localOrbit;
    // Start is called before the first frame update
    public override void NetworkStart()
    {
        base.NetworkStart();
        if (!IsServer)
            return;
        _tr = transform;
        _initialPos = _tr.position;
        Random.InitState((int)_initialPos.x * 10 * (int)_tr.localScale.x +
                         (int)_initialPos.y * 10 * (int)_tr.localScale.y + (int)_initialPos.z * 10 * (int)_tr.localScale.z);
        _localOrbit = new Vector3(Random.Range(-35f, 35f),Random.Range(-35f, 35f),Random.Range(-35f, 35f));
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer)
            return;
        _tr.position = /*Time.deltaTime * speed **/ new Vector3(Mathf.Sin(Time.time * speed) * Mathf.Cos(Time.time * speed) * _localOrbit.x,
            Mathf.Sin(Time.time * speed) * Mathf.Sin(Time.time * speed) * _localOrbit.y, Mathf.Cos(Time.time * speed) * _localOrbit.z) + _initialPos;
    }

   
}
