using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject root;
    private RaycastHit _hit = new RaycastHit();
    private LineRenderer _line;
    private Vector3[] _pos;
    private Vector3 _nextPos = Vector3.zero;
    private Coroutine _jump = null;
    private Vector3 _nextNormal = Vector3.zero;
    private float _distance;
    private GameObject _tmpPlanet;

    public float speed = 1;

    private Transform _tr;

    // Start is called before the first frame update
    void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _pos = new Vector3[2];
        _tr = transform;
        _line.positionCount = 2;
        _line.enabled = false;
    }

    IEnumerator SwitchPlanet()
    {
        Player.Instance.currentPlanet = null;
        Vector3 tmpNormal = root.transform.up;
        Vector3 tmpPos = root.transform.position;
        float timeElapsed = 0;
        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * speed * _tmpPlanet.transform.localScale.x / _distance;
            root.transform.up = Vector3.Slerp(tmpNormal, _nextNormal, timeElapsed);
            root.transform.position = Vector3.Slerp(tmpPos, _nextPos + _tmpPlanet.transform.position, timeElapsed);
            yield return null;
        }
        root.transform.parent = _tmpPlanet.transform;
        Player.Instance.currentPlanet = _tmpPlanet;
    }

// Update is called once per frame
    void Update()
    {
        if (Input.GetKey("joystick button 15"))
        {
            if (Physics.Raycast(transform.position, transform.forward, out _hit))
            {
                _line.enabled = true;
                _pos[0] = transform.position;
                _pos[1] = _hit.point;
                _line.SetPositions(_pos);
                if (Input.GetKeyDown("joystick button 5"))
                {
                    
                    _nextPos = _hit.point - _hit.collider.gameObject.transform.position;
                    _nextNormal = _hit.normal;
                    _distance = _hit.distance;
                    _tmpPlanet = _hit.collider.gameObject;
                    if (_jump != null)
                    {
                        StopCoroutine(_jump);
                        _jump = null;
                    }
                    _jump = StartCoroutine(SwitchPlanet());
                }
            }
            else
            {
                _line.enabled = false;
            }
            Debug.DrawLine(transform.position, _tr.position + _tr.forward * 25f);
        }
        else
        {
            _line.enabled = false;
        }
    }
}
