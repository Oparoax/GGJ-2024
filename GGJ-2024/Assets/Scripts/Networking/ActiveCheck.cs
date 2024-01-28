using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class IfOwnerNotActive : NetworkBehaviour
{
    [SerializeField] GameObject ObjToTurnOn;


    public void OnStartClient()
    {
        if (!ObjToTurnOn.activeInHierarchy)
        {
            ObjToTurnOn.SetActive(true);
        }
    }
}
