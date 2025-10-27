using System;
using System.Collections;
using TMPro;
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
        _agent.speed = _speed;
    }

    public IEnumerator Move(Vector3 targetPosition, float distanceError = 0f)
    {
        if(distanceError == 0)
            distanceError = _distanceError;

        _agent.SetDestination(targetPosition);

        while (Vector3.SqrMagnitude(transform.position - targetPosition) > distanceError)
            yield return null;

        _agent.ResetPath();
    }

    public void SetZeroVelocity()
    {
        _agent.speed = 0f;
    }

    public void ResetVelocity()
    {
        _agent.speed = _speed;
    }
}
