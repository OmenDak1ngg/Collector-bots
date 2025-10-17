using System;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private readonly KeyCode ScanKey = KeyCode.LeftAlt;
    private readonly KeyCode DisplayFlagKey = KeyCode.LeftControl;

    private bool _isFlagDisplayed;

    public event Action Scanned;
    public event Action DisplayedFlag;
    public event Action HidedFlag;

    private void Awake()
    {
        _isFlagDisplayed = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(ScanKey))
        {
            Scanned?.Invoke();
        }

        while (Input.GetKeyDown(DisplayFlagKey))
        {
            _isFlagDisplayed = true;
            DisplayedFlag?.Invoke();
        } 

        if(_isFlagDisplayed && Input.GetKeyUp(DisplayFlagKey))
        {
            HidedFlag?.Invoke();
            _isFlagDisplayed = false;
        }
    }
}
