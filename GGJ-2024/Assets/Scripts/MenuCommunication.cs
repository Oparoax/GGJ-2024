using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class MenuCommunication : NetworkBehaviour
{
    public UIHandler UIHandlerSC;


    public override void OnStartClient()
    {
        base.OnStartClient();
   
        UIHandlerSC = GameObject.Find("MenuHandler").GetComponent<UIHandler>();

        if (UIHandlerSC.player == null)
        {
            UIHandlerSC.player = this.gameObject;
        }

        RPCSetPlayers();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RPCSetPlayers()
    {
        if (UIHandlerSC.player1 == null)
        {
            UIHandlerSC.isPlayer1 = true;
            UIHandlerSC.player1 = this.gameObject;
            
        }
        else if (UIHandlerSC.player2 == null)
        {
            UIHandlerSC.isPlayer2 = true;
            UIHandlerSC.player2 = this.gameObject;
            
        }
        SetPlayers();
    }

    [ObserversRpc]
    private void SetPlayers()
    {
        
    }
}
