using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    const float forceCoef = 10;
    [SerializeField] Transform Transform;
    public void Throw(float force, Vector3 direction)
    {
        rb.isKinematic = false;
        rb.AddForce(direction * (force * forceCoef), ForceMode.Impulse);
    }
}
