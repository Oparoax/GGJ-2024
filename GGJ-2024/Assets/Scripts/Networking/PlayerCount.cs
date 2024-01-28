using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public class PlayerCount : NetworkBehaviour
{

    [SyncVar] public bool isPlayer1;
    [SyncVar] public bool isPlayer2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
