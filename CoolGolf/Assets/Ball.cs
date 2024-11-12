using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    [SerializeField] Transform Transform;
    [SerializeField] MeshRenderer meshRenderer;
    public bool isEnd = false;
    public void Throw(Vector3 direction, float force)
    {
        rb.isKinematic = false;
        rb.AddForce(direction * force, ForceMode.VelocityChange);
    }
    public void StopBall()
    {
        rb.isKinematic = true;
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
}
