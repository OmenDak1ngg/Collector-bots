using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : Spawner<Robot>
{
    [SerializeField] private int _startRobots = 3;
    [SerializeField] private SpawnZone _spawnZone;
    [SerializeField] private int _maxRobots;
    [SerializeField] private ErrorViewer _errorViewer;
    [SerializeField] private ResourceTracker _resourceTracker;

    private readonly string _noSpaceText = "нет свободного места для робота";

    private float _spawnDelay = 0.2f;

    private WaitForSeconds _waitSpawn;

    public event Action<Robot> RobotCreated;

    protected override void Awake()
    {
        _waitSpawn = new WaitForSeconds(_spawnDelay);

        base.Awake();
    }

    private void Start()
    {
        SpawnStartRobots();
    }

    private void SpawnStartRobots()
    {
        StartCoroutine(SpawnRobot());
    }

    private IEnumerator SpawnRobot()
    {
        for (int i = 0; i < _startRobots; i++)
        {
            if (_startRobots > _maxRobots)
                _startRobots = _maxRobots;

            Pool.Get();

            yield return _waitSpawn;
        }
    }

    protected override Robot OnInstantiate()
    {
        Robot robot = base.OnInstantiate();

        RobotCreated?.Invoke(robot);

        return robot;
    }

    protected override void OnGet(Robot pooledObject)
    {
        if (pooledObject == null)
            return;

        Vector3 spawnpoint = _spawnZone.GetRandomAvalaibleSpawnpoint();

        if (spawnpoint == Vector3.zero)
        {
            _errorViewer.ShowText(_noSpaceText);
            Pool.Release(pooledObject);
            return;
        }

        pooledObject.SetSpawnpointPosition(spawnpoint);
        pooledObject.transform.position = spawnpoint;

        base.OnGet(pooledObject);
    }
}
