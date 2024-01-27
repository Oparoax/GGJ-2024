using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.BalloonCollected();

            // May need to use destroy if too many balloons in scene.

            RPCUpdateObjState();
        }
    }

    [ServerRpc(RequireOwnership = true)] private void RPCUpdateObjState()
    {
        UpdateObjState();
    }

    [ObserversRpc] private void UpdateObjState()
    {
        gameObject.SetActive(false);
    }
}
