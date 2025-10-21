using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private RobotSpawner _robotSpawner;
    [SerializeField] private UserInput _userInput;
    [SerializeField] private ErrorViewer _errorViewer;
    [SerializeField] private Storage _storage;
    [SerializeField] private ResourceTracker _resourceTracker;

    private readonly string _noResourceText = "не найдено ни одного ресурса";
    private readonly string _noRobotsText = "нет ни одного свободного робота";

    private int _startRobots = 3;
    private List<Robot> _robots;

    public event Action<Vector3> RobotReachedFlag;
    
    private void OnEnable()
    {
        _scanner.ResourceFounded += SendRobotForResources;
        _robotSpawner.RobotCreated += SetupRobotForStorage;
        _storage.ResourceAdded += _resourceTracker.UnMarkTaked;
        _storage.CollectedThreeResources += CreateRobot;
        _userInput.PlacedFlag += SendRobotToFlag;
    }

    private void OnDisable()
    {
        _scanner.ResourceFounded -= SendRobotForResources;
        _robotSpawner.RobotCreated -= SetupRobotForStorage;
        _storage.ResourceAdded -= _resourceTracker.UnMarkTaked;
        _storage.CollectedThreeResources -= CreateRobot;
        _userInput.PlacedFlag -= SendRobotToFlag;

        foreach(Robot robot in _robots)
        {
            robot.ReachedFlag -= OnRobotReachedFlag;
        }
    }

    private void Awake()
    {
        _robots = new List<Robot>();
    }

    private void Start()
    {
        for(int i = 0;i < _startRobots; i++)
        {
            CreateRobot();
        }
    }

    private void SetupRobotForStorage(Robot robot)
    {
        robot.SetStoragePosition(_storage.transform.position);

        _robots.Add(robot);
        robot.ReachedFlag += OnRobotReachedFlag;
    }

    private void SendRobotToFlag(Vector3 flagPosition)
    {
        foreach(Robot robot in _robots)
        {
            if (robot.IsBusy)
                continue;

            robot.StartMoveToFlag(flagPosition);
            break;
        }
    }

    private void CreateRobot()
    {
        _robotSpawner.Get();
    }

    private void OnRobotReachedFlag(Vector3 flagPosition)
    {
        RobotReachedFlag?.Invoke(flagPosition);
    }

    public void SendRobotForResources(Resource resource)
    {
        _resourceTracker.MarkTaked(resource);

        if (resource == null)
        {
            _errorViewer.ShowText(_noResourceText);
            return;
        }

        foreach (Robot robot in _robots)
        {
            if(robot.IsBusy)
                continue;

            robot.StartMoveToResource(resource);

            return;
        }

        _errorViewer.ShowText(_noRobotsText);
    }
}
