using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    public Collider Collider { get; private set; }  
    public Rigidbody Rigidbody { get; private set; }

    public bool Collected { get; private set; } 

    public event Action<Resource> ReachedStorage;

    private void Awake()
    {
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
        Collected = false;  
    }

    public void InvokeReachedStorage()
    {
        ReachedStorage?.Invoke(this);
    }

    public void MarkCollected()
    {
        Collected = true;
    }

    public void UnMarkCollected()
    {
        Collected = false;
    }
}
