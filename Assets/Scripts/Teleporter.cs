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
    private Vector3 nextPos = Vector3.zero;
    private Coroutine jump = null;
    private Vector3 nextNormal = Vector3.zero;
    private float distance;
    private GameObject tmpPlanet;

    public float speed = 1;
    // Start is called before the first frame update
    void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _pos = new Vector3[2];
        _line.positionCount = 2;
        _line.enabled = false;
    }

    IEnumerator SwitchPlanet()
    {
        Vector3 tmpNormal = root.transform.up;
        Vector3 tmpPos = root.transform.position;
        // Tween tween = root.transform.DOMove(nextPos + tmpPlanet.transform.position, distance / 10f).SetAutoKill();
        float timeElapsed = 0;
        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * speed * tmpPlanet.transform.localScale.x / distance;
            root.transform.up = Vector3.Slerp(tmpNormal, nextNormal, timeElapsed);
            root.transform.position = Vector3.Slerp(tmpPos, nextPos + tmpPlanet.transform.position, timeElapsed);
            yield return null;
        }
        root.transform.parent = tmpPlanet.transform;
        Player.Instance.currentPlanet = tmpPlanet;
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
                    
                    nextPos = _hit.point - _hit.collider.gameObject.transform.position;
                    nextNormal = _hit.normal;
                    distance = _hit.distance;
                    tmpPlanet = _hit.collider.gameObject;
                    if (jump != null)
                    {
                        StopCoroutine(jump);
                        jump = null;
                    }
                    jump = StartCoroutine(SwitchPlanet());
                }
            }
            else
            {
                _line.enabled = false;
            }
            Debug.DrawLine(transform.position, transform.position + transform.forward * 25f);
        }
        else
        {
            _line.enabled = false;
        }
    }
}
