using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class NM_PlayerController : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 10;
    [SerializeField] private LayerMask _layerMask;

    private enum PLAYERMOVEMENT {   NONE, MANUAL, CLICK     }
    PLAYERMOVEMENT _currentmovement = PLAYERMOVEMENT.NONE;

    private Camera _cam;
    private NavMeshAgent _agent;

    private float h;
    private float v;
    private Vector3 _playerInput;



    private void Awake()
    {
        _cam = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        transform.forward = _cam.transform.forward;
        transform.right = _cam.transform.right;
    }

    private void Update()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0)) 
        {
            Ray ray = _cam.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _layerMask)) 
            {
                _agent.SetDestination(hit.point);
                _currentmovement = PLAYERMOVEMENT.CLICK;
            }
        }

        h = UnityEngine.Input.GetAxis("Horizontal");
        v = UnityEngine.Input.GetAxis("Vertical");
        if (h != 0 || v != 0)
        {
            _playerInput = new Vector3(h, 0, v);
            _agent.ResetPath();
            _currentmovement = PLAYERMOVEMENT.MANUAL;

        }
        else
        {
            _playerInput = Vector3.zero;
            _currentmovement= PLAYERMOVEMENT.NONE;
        }
        if (_currentmovement == PLAYERMOVEMENT.MANUAL)
        {
            Vector3 camForward = _cam.transform.forward;
            Vector3 camRight = _cam.transform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = (camForward * _playerInput.z + camRight * _playerInput.x).normalized;

            // Muove l'agente in tempo reale, rispettando il NavMesh
            _agent.Move(moveDir * _agent.speed * Time.deltaTime);

            // Ruota il personaggio nella direzione di movimento
            if (moveDir != Vector3.zero)
                transform.forward = moveDir;
        }
    }

}
