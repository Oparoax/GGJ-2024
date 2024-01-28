using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FishNet.Component.Animating;
using FishNet.Object;

public class PlayerModel : NetworkBehaviour
{
    public bool blue, red;
    public NetworkAnimator _BlueNetworkAnimator, _RedNetworkAnimator;
    public GameObject blueModel, redModel;

    public PlayerCount playerCountSC;

    public override void OnStartClient()
    {
        base.OnStartClient();

        playerCountSC = GameObject.Find("PlayerCounter").GetComponent<PlayerCount>();

        Debug.Log("Starting 2 seconds");
        StartCoroutine(WaitForSeconds(2f));
    }


    public IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        RPCPlayerSet();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RPCPlayerSet()
    {
        SetPlayers();
    }


    [ObserversRpc]
    private void SetPlayers()
    {
        Debug.Log("SET PLAYERS CALLED");
        if (playerCountSC.isPlayer1)
        {
            Debug.Log("Turning on red");
            this.gameObject.GetComponent<PlayerController>().red = true;
            this.gameObject.GetComponent<PlayerController>().redModel.SetActive(true);
        }
        else if (playerCountSC.isPlayer2)
        {
            Debug.Log("Turning on blue");
            this.gameObject.GetComponent<PlayerController>().red = false;
            this.gameObject.GetComponent<PlayerController>().redModel.SetActive(false);

        }
    }
}
