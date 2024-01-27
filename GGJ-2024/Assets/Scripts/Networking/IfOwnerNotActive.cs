using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class IfOwnerNotActive : NetworkBehaviour
{
    public void OnStartClient()
    {
        if (!IsOwner)
        {
            this.gameObject.SetActive(false);
        }
    }
}
