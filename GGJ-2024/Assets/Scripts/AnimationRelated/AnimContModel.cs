using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimContModel : MonoBehaviour
{
    public PlayerController playerControllerSC;
    private Animator _animator;

    private void Start()
    {
        _animator = this.gameObject.GetComponent<Animator>();
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
