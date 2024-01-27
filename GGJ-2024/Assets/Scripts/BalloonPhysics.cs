using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonPhyscis : MonoBehaviour
{
    Rigidbody rb;

    //Vector3 downForce = new Vector3(0, -0.4f, 0);
    Vector3 upForce = new Vector3(0, 0.9f, 0);

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddForce(upForce, ForceMode.Force);
        //rb.AddForce(downForce, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rb.AddForce(Vector3.up * 2, ForceMode.Impulse);
        }
    }
}
