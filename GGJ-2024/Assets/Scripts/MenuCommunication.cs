using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class MenuCommunication : NetworkBehaviour
{
    public UIHandler UIHandlerSC;
    //public PlayerCount PlayerCountSC;

    public override void OnStartClient()
    {
        //PlayerCountSC = GameObject.Find("PlayerCounter").GetComponent<PlayerCount>();
        UIHandlerSC = GameObject.Find("MenuHandler").GetComponent<UIHandler>();

        if (UIHandlerSC.player == null)
        {
            UIHandlerSC.player = this.gameObject;
        }

        if (IsOwner)
        {
            Debug.Log("Setting player numbers");
            RPCSetPlayers();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void RPCSetPlayers()
    {
        if (UIHandlerSC.player1 == null)
        {
            UIHandlerSC.isPlayer1 = true;
            UIHandlerSC.player1 = this.gameObject;
            //PlayerCountSC.isPlayer1 = true;

            //SetPlayers1();
        }
        else if (UIHandlerSC.player2 == null)
        {
            UIHandlerSC.isPlayer2 = true;
            UIHandlerSC.player2 = this.gameObject;
            //PlayerCountSC.isPlayer2 = true;

            //SetPlayers2();
            
        }

        //SetPlayers();
    }

    [ObserversRpc]
    private void SetPlayers1()
    {
        this.gameObject.GetComponent<PlayerModel>().blue = true;
            
    }

    [ObserversRpc]
    private void SetPlayers2()
    {
        this.gameObject.GetComponent<PlayerModel>().red = true;
    }
}
