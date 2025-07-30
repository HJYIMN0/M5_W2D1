using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerWarper : MonoBehaviour
{
    [SerializeField] private NavMeshAgent[] _agents;
    [SerializeField] private Transform _transform;

    private Vector3[] _startPos;

    private void Awake()
    {
        _startPos = new Vector3[_agents.Length];

        if (_agents != null && _agents.Length >= 0) 
        {
            for (int i = 0; i < _agents.Length; i++) 
            {
                _agents[i] = _agents[i].GetComponent<NavMeshAgent>();
                _startPos[i] = _agents[i].gameObject.transform.position;

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {      

        NavMeshAgent agent = other.gameObject.GetComponentInParent<NavMeshAgent>();
        Debug.Log(agent);
       if (agent != null && _transform != null)
       {
            agent.Warp(_transform.position);
            agent.ResetPath();
           
       }
     
    }
}
