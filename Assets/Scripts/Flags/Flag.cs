using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Flag : MonoBehaviour
{
    private BoxCollider _boxCollider;

    public event Action RobotArrived;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Robot>(out _))
        {
            RobotArrived?.Invoke();
        }
    }

    public void SetColliderSize(float size)
    {
        _boxCollider.size = new Vector3(size/2, size/2, size/2);
    }
}
