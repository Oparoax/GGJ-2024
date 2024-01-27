using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using Cinemachine;

public class IfOwnerActive : NetworkBehaviour
{
    [SerializeField] Camera MainCam;
    [SerializeField] CinemachineVirtualCamera VirtualCamera;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (IsOwner)
        {
            Debug.Log("*********************" + IsOwner);
            MainCam.enabled = true;
            VirtualCamera.enabled = true;
        }
    }
}
