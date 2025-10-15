using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RobotMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceError;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public IEnumerator Move(Vector3 targetPosition)
    {
        _agent.SetDestination(targetPosition);

        while (Vector3.SqrMagnitude(transform.position - targetPosition) > _distanceError)
            yield return null;

        _agent.ResetPath();
    }
}
