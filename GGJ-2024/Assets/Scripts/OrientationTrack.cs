using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationTrack : MonoBehaviour
{
    public Transform camera;

    void Update()
    {
        this.gameObject.transform.eulerAngles = new Vector3(0, camera.eulerAngles.y, 0f);
    }
}
