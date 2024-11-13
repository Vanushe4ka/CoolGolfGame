using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] Transform Transform;
    [SerializeField] MeshRenderer meshRenderer;
    public bool isEnd = false;
    static float sleepThreshold;
    public bool isOutOfBounds = false;
    public void SetSleepThreshold(float newVal)
    {
        sleepThreshold = newVal;
        rb.sleepThreshold = sleepThreshold;
    }
    private void Start()
    {
        rb.sleepThreshold = sleepThreshold;
    }
    public void Throw(Vector3 direction, float force)
    {
        rb.isKinematic = false;
        rb.AddForce(direction * force, ForceMode.VelocityChange);
    }
    public void StopBall()
    {
        rb.isKinematic = true;
    }
    private void FixedUpdate()
    {
        CheckBounds();
    }
    public void CheckBounds()
    {
        if (!GameController.Instance().IsPointInBounds(transform.position))
        {
            StopBall();
            meshRenderer.enabled = false;
            isOutOfBounds = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lunka")
        {
            isEnd = true;
            meshRenderer.enabled = false;
            StopBall();
        }
    }
    public void ResetAfterOutOfBounds()
    {
        meshRenderer.enabled = true;
        isOutOfBounds = false;
    }
}
