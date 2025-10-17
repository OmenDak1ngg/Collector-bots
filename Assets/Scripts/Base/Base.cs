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

    private List<Robot> _robots;

    private void OnEnable()
    {
        _userInput.Scanned += SendRobotForResources;
        _robotSpawner.RobotCreated += SetupRobotForStorage;
        _storage.ResourceAdded += _resourceTracker.UnMarkTaked;
        _storage.CollectedThreeResources += CreateRobot;
    }

    private void OnDisable()
    {
        _userInput.Scanned -= SendRobotForResources;
        _robotSpawner.RobotCreated -= SetupRobotForStorage;
        _storage.ResourceAdded -= _resourceTracker.UnMarkTaked;
        _storage.CollectedThreeResources -= CreateRobot;
    }

    private void Awake()
    {
        _robots = new List<Robot>();
    }

    private void SetupRobotForStorage(Robot robot)
    {
        robot.SetStoragePosition(_storage.transform.position);

        _robots.Add(robot);
    }

    private void CreateRobot()
    {
        _robotSpawner.Get();
    }

    public void SendRobotForResources()
    {
        Resource closestResource = _scanner.GetClosestResource();

        _resourceTracker.MarkTaked(closestResource);

        if (closestResource == null)
        {
            _errorViewer.ShowText(_noResourceText);
            return;
        }

        foreach (Robot robot in _robots)
        {
            if(robot.IsBusy)
                continue;

            robot.StartMoveToResource(closestResource);

            return;
        }

        _errorViewer.ShowText(_noRobotsText);
    }
}
