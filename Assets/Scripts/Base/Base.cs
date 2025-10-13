using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private RobotSpawner _robotSpawner;
    [SerializeField] private UserInput _userInput;
    [SerializeField] private ErrorViewer _errorViewer;

    private readonly string _noResourceText = "не найдено ни одного ресурса";
    private readonly string _noRobotsText = "нет ни одного свободного робота";

    private void OnEnable()
    {
        _userInput.Scanned += SendRobotForResources;
    }

    private void OnDisable()
    {
        _userInput.Scanned -= SendRobotForResources;
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

            robot.GetComponent<RobotMover>().StartMoveToResource(closestResource);

            robot.SetBusy();
            return;
        }

        _errorViewer.ShowText(_noRobotsText);
    }
}
