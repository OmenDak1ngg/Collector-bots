using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public event Action RobotArrived; 

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Robot>(out _))
        {
            RobotArrived?.Invoke();
        }
    }
}