using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Component.Animating;

public class AnimContModel : MonoBehaviour
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
            _animator.SetBool("Walking", true);
        }
        else
        {
            _animator.SetBool("Walking", false);
        }
    }
}
