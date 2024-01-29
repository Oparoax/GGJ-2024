using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    [SerializeField] GameObject SelfIgnore;

    [SerializeField] GameObject forceObj;

    public float multiplyer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject != SelfIgnore)
        {
            Debug.Log("Hit:" + other.gameObject.name);
            other.GetComponent<Rigidbody>().AddForce(SelfIgnore.transform.forward * 50f, ForceMode.Impulse);
        }

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Rigidbody>().AddForce(forceObj.transform.forward * multiplyer, ForceMode.Impulse);
        }
    }
}
