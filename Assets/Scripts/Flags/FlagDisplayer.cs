using System.Collections.Generic;
using UnityEngine;

public class FlagDisplayer : MonoBehaviour
{
    [SerializeField] private UserInput _userInput;

    private List<Flag> _flags;

    private void OnEnable()
    {
        _userInput.DisplayedFlag += StartDisplayFlag;
        _userInput.HidedFlag += StopDisplayFlag;

        foreach(Flag flag in _flags)
        {
            flag.RobotArrived += StopDisplayFlag;
        }
    }

    private void OnDisable()
    {
        _userInput.DisplayedFlag -= StartDisplayFlag;
        _userInput.HidedFlag -= StopDisplayFlag;
        
        foreach(Flag flag in _flags)
        {
            flag.RobotArrived -= StopDisplayFlag;
        }
    }

    private void Awake()
    {
        _flags = new List<Flag>();
    }

    public void StartDisplayFlag(Flag flag)
    {
        flag.RobotArrived += StopDisplayFlag;
        _flags.Add(flag);
        flag.gameObject.SetActive(true);
    }

    public void StopDisplayFlag(Flag flag)
    {
        flag.gameObject.SetActive(false);
    }
}