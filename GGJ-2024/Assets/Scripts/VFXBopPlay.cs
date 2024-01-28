using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXBopPlay : MonoBehaviour
{
    public ParticleSystem BopVFX;

    public void BopPlayVFX()
    {
        BopVFX.Play();
    }
}
