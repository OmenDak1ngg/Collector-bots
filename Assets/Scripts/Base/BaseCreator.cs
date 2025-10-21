using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BaseCreator : MonoBehaviour
{
    [SerializeField] private Base _prefab;
    [SerializeField] private Base _startBase;

    private List<Base> _bases;

    private void OnEnable()
    {
        _startBase.RobotReachedFlag += CreateBase;
    }

    private void OnDisable()
    {
        foreach(Base robotBase in _bases)
        {
            robotBase.RobotReachedFlag -= CreateBase;
        }
    }

    public void CreateBase(Vector3 flagPosition)
    {
        Base newBase = Instantiate(_prefab);
        newBase.RobotReachedFlag += CreateBase;
        newBase.transform.position = flagPosition;

        _bases.Add(newBase);
    }
}
