using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    public float timeStep = 0.1f;
    public Vector3 lastPoint;

    public void HideLine()
    {
        line.positionCount = 0;
    }
    public void DrawLine(Vector3 startPoint, Vector3 direction, float force, Ball ballPrefab)
    {
        List<Vector3> points = PredictTrajectory(startPoint, direction, force, ballPrefab);
        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
        lastPoint = points[points.Count - 1];
    }
    public List<Vector3> PredictTrajectory(Vector3 startPoint, Vector3 direction, float force, Ball ballPrefab)
    {
        GameObject ballObj = Instantiate(ballPrefab.gameObject, startPoint, Quaternion.identity);
        Ball ball = ballObj.GetComponent<Ball>();
        ball.Throw(direction, force);
        Physics.autoSimulation = false;
        int i = 0;
        List<Vector3> points = new List<Vector3>();
        while (i < 10 || !ball.rb.IsSleeping())
        {
            i++;
            points.Add(ballObj.transform.position);
            ball.CheckBounds();
            Physics.Simulate(timeStep);
        }
        Physics.autoSimulation = true;
        Destroy(ballObj);
        return points;
    }
}
