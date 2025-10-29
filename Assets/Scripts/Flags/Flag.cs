using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Flag : MonoBehaviour
{
    private BoxCollider _boxCollider;

    public bool IsFlagPlaced { get; private set; }

    public event Action<Flag> RobotArrived;

    private void Awake()
    {
        IsFlagPlaced = false;
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void InvokeRobotArrived()
    {
        RobotArrived?.Invoke(this);
    }

    public void SetColliderSize(float size)
    {
        _boxCollider.size = new Vector3(size/2, size/2, size/2);
    }

    public void MarkPlaced()
    {
        IsFlagPlaced = true;
    }

    public void UnMarkFlagPlaced()
    {
        IsFlagPlaced = false;
    }
}
