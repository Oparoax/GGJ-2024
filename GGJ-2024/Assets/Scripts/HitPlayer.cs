using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    [SerializeField] GameObject SelfIgnore;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject != SelfIgnore)
        {
            Debug.Log("Hit:" + other.gameObject.name);
            other.GetComponent<Rigidbody>().AddForce(SelfIgnore.transform.forward * 50f, ForceMode.Impulse);
        }
    }
}
