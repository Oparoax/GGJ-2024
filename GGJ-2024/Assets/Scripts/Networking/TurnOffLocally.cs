using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class TurnOffLocally : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            this.gameObject.SetActive(false);
        }

    }

}
