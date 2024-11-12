using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] int DefaultPointsCount;
    public int poitsCount;
    public float timeStep = 0.1f;
    public Vector3 lastPoint;
    public float minMagnitude;
    public void SetPCThrowForce(float force)
    {
        poitsCount = Mathf.RoundToInt(DefaultPointsCount * force);
    }
    public void PredictTrajectory(Vector3 startPoint, Vector3 direction, float force, Ball ballPrefab)
    {
        GameObject ballObj = Instantiate(ballPrefab.gameObject, startPoint, Quaternion.identity);
        Ball ball = ballObj.GetComponent<Ball>();
        ball.Throw(direction, force);
        Physics.autoSimulation = false;
        int i = 0;
        List<Vector3> points = new List<Vector3>();
        while(i < 10 || ball.rb.velocity.magnitude > minMagnitude)
        {
            i++;
            points.Add(ballObj.transform.position);
            Physics.Simulate(timeStep);
        }
        Physics.autoSimulation = true;
        Destroy(ballObj);
        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
        lastPoint = points[points.Count - 1];
    }

}
