using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementDirection
{
    Up, 
    Down, 
    Left, 
    Right
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRB;

    public PlayerInput _playerInput;

    public InputAction _action;

    [SerializeField]
    private InputActionReference _forwardAction;
    private InputActionReference _backAction;
    private InputActionReference _leftAction;
    private InputActionReference _rightAction;

    // Start is called before the first frame update
    void Start()
    {
        if (_playerRB == null)
        {
            _playerRB = GetComponent<Rigidbody>();
        }

        _action = _playerInput.actions["Movement"];
    }

    public void MovePlayer()
    {
    }

    void Move(Vector3 force)
    {
        _playerRB.AddForce(force, ForceMode.Impulse);
    }

}
