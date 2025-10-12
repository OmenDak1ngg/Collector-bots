using System;
using System.Collections;
using UnityEngine;
public class UserInput : MonoBehaviour
{
    private readonly KeyCode ScanKey = KeyCode.LeftAlt;

    public event Action Scanned;

    private void Update()
    {
        if (Input.GetKeyUp(ScanKey))
        {
            Scanned?.Invoke();
        }
    }
}
