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

    private void OnEnable()
    {
        _userInput.Scanned += SendRobotForResources;
        _robotSpawner.RobotCreated += SetupRobotForStorage;
    }

    private void OnDisable()
    {
        _userInput.Scanned -= SendRobotForResources;
        _robotSpawner.RobotCreated -= SetupRobotForStorage;
    }
    
    private void SetupRobotForStorage(Robot robot)
    {
        robot.Mover.SetStorage(_storage.transform.position);
        robot.Mover.SetResourceTracket(_resourceTracker);
        robot.ResourceGrabber.SetResourceTracker(_resourceTracker);
    }

    public void SendRobotForResources()
    {
        List<Robot> robots = _robotSpawner.AvalaibleRobots;

        Resource closestResource = _scanner.GetClosestResource();

        if (closestResource == null)
        {
            _errorViewer.ShowText(_noResourceText);
            return;
        }

        foreach (Robot robot in robots)
        {
            if(robot.IsBusy)
                continue;

            robot.MoveToResource(closestResource);

            robot.MarkBusy();
            return;
        }

        _errorViewer.ShowText(_noRobotsText);
    }
}
