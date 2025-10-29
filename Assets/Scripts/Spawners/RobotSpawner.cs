using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : Spawner<Robot>
{
    [SerializeField] private SpawnZone _spawnZone;
    [SerializeField] private int _maxRobots;
    
    private readonly string _noSpaceText = "нет свободного места для робота";
    private readonly string _maxRobotsText = "превышено максимальное количетсво робото";

    private ErrorViewer _errorViewer;
    private int _countOfRobots;
    public event Action<Robot> RobotCreated;

    protected override Robot OnInstantiate()
    {
        Robot robot = base.OnInstantiate();

        RobotCreated?.Invoke(robot);

        return robot;
    }

    protected override void OnGet(Robot pooledObject)
    {
        if (_countOfRobots >= _maxRobots)
        {
            _errorViewer.ShowText(_maxRobotsText);
            return;
        }

        if (pooledObject == null)
            return;

        Vector3 spawnpoint = _spawnZone.GetRandomAvalaibleSpawnpoint();

        if (spawnpoint == Vector3.zero)
        {
            _errorViewer.ShowText(_noSpaceText);
            Pool.Release(pooledObject);
            return;
        }

        pooledObject.transform.parent = _spawnZone.transform;
        pooledObject.SetSpawnpointPosition(spawnpoint);
        pooledObject.transform.position = spawnpoint;

        base.OnGet(pooledObject);
    }

    public void SetupOnCreate(ErrorViewer errorViewer)
    {
        _errorViewer = errorViewer;
    }
}
