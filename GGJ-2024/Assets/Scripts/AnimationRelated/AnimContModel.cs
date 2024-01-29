using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Component.Animating;

public class AnimContModel : NetworkBehaviour
{
    public PlayerController playerControllerSC;
    private Animator _animator;
    private NetworkAnimator _networkAnimator;

    private void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
        _networkAnimator = this.gameObject.GetComponent<NetworkAnimator>();
    }
    
    void Update()
    {
        if (playerControllerSC.isMoving)
        {
            RPCWalkStateOn();
            //_animator.SetBool("Walking", true);
        }
        else
        {
            RPCWalkStateOff();
            //_animator.SetBool("Walking", false);
        }
    }

    [ServerRpc] private void RPCWalkStateOn()
    {
        WalkStateOn();
    }

    [ObserversRpc] private void WalkStateOn()
    {
        _animator.SetBool("Walking", true);
    }

    [ServerRpc]
    private void RPCWalkStateOff()
    {
        WalkStateOff();
    }

    [ObserversRpc]
    private void WalkStateOff()
    {
        _animator.SetBool("Walking", false);
    }
}
