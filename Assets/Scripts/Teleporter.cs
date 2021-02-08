using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool canTeleport = false;
    private bool aim = false;

    private PlayerInputLite inputs;
    // Start is called before the first frame update
    void Start()
    {
        inputs = PlayerInputLite.Instance;
        _line = GetComponent<LineRenderer>();
        _pos = new Vector3[2];
        _tr = transform;
        _line.positionCount = 2;
        _line.enabled = false;
        if (inputs == null)
            return;
        inputs.CreateAction("teleport", PlayerInputLite.Button.gripPressed, PlayerInputLite.TypeHand.RightHand,
            PlayerInputLite.TypeController.XRController, InputActionType.Button, 
            PlayerInputLite.InteractionType.PressAndRelease).performed += OnTeleport;
        InputAction action = inputs.CreateAction("aim", PlayerInputLite.Button.triggerPressed,
            PlayerInputLite.TypeHand.RightHand, PlayerInputLite.TypeController.XRController, InputActionType.Button,
            PlayerInputLite.InteractionType.PressOnly);
            action.started += OnAim;
        // inputs.CreateAction("release", PlayerInputLite.Button.triggerPressed, PlayerInputLite.TypeHand.RightHand,
            // PlayerInputLite.TypeController.XRController, InputActionType.Button,
            // PlayerInputLite.InteractionType.ReleaseOnly)
            action.canceled += OnRelease;
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

    public void OnTeleport(InputAction.CallbackContext context)
    {
        if (!canTeleport)
            return;
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
    
    public void OnAim(InputAction.CallbackContext context)
    {
        aim = true;
        
    }

    public void OnRelease(InputAction.CallbackContext context)
    {
        aim = false;
        _line.enabled = false;
    }
    

// Update is called once per frame
    void Update()
    {
        if (aim)
        {
            if (Physics.Raycast(transform.position, transform.forward, out _hit))
            {
                _line.enabled = true;
                _pos[0] = transform.position;
                _pos[1] = _hit.point;
                _line.SetPositions(_pos);
                canTeleport = true;
            }
            else
            {
                canTeleport = false;
                _line.enabled = false;
            }
        }
    }
}
