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
    private readonly string _maxRobotsText = "превышено макс кол-во роботов";

    private float _spawnDelay = 0.2f;

    private WaitForSeconds _waitSpawn;

    public List<Robot> AvalaibleRobots { get; private set; }

    public event Action<Robot> RobotCreated;

    protected override void Awake()
    {
        _waitSpawn = new WaitForSeconds(_spawnDelay);

        base.Awake();
    }

    private void Start()
    {
        AvalaibleRobots = new List<Robot>();
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
        if (AvalaibleRobots.Count >= _maxRobots)
        {
            _errorViewer.ShowText(_maxRobotsText);

            return null;
        }

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

        pooledObject.GetComponent<RobotMover>().SetSpawnpoint(spawnpoint);
        pooledObject.transform.position = spawnpoint;

        base.OnGet(pooledObject);

        AvalaibleRobots.Add(pooledObject);

    }

    protected override void OnRelease(Robot pooledObject)
    {
        base.OnRelease(pooledObject);

        AvalaibleRobots.Remove(pooledObject);
    }
}
