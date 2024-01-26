using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using FixedUpdate = UnityEngine.PlayerLoop.FixedUpdate;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRb;

    public Transform Orientation;

    public Camera PlayerCamera;

    [SerializeField] LayerMask floorMask;

    public PlayerInput PlayerInput;
    public InputAction MoveAction;
    public InputActionReference JumpAction;

    public bool IsMoving;
    public bool IsGrounded;

    // Start is called before the first frame update
    void Start()
    {
        if (_playerRb == null)
        {
            _playerRb = GetComponent<Rigidbody>();
        }

        MoveAction = PlayerInput.actions["Movement"];
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Jump();

        IsGrounded = GroundCheck();

        _playerRb.drag = IsGrounded ? 0.5f : 0f;
    }

    RaycastHit _raycastHit;

    private bool GroundCheck()
    {
        return Physics.Raycast(gameObject.transform.position, Vector3.down, out _raycastHit, 2f * 0.5f + 0.2f,
            floorMask);
    }

    public float ForceModifier = 5f;
    public float MoveSpeed = 5f;

    public void MovePlayer()
    {
        Vector2 movement = MoveAction.ReadValue<Vector2>();

        if (movement != Vector2.zero)
        {
            IsMoving = true;

            float movHor = movement.x;
            float movVert = movement.y;

            Vector3 inputDir = Orientation.forward * movVert + Orientation.right * movHor;

            Move(MoveSpeed, inputDir, ForceMode.Force);
        }
        else
        {
            IsMoving = false;
        }
    }

    public float JumpForce = 5f;

    public void Jump()
    {
        bool jump = JumpAction.action.triggered;

        Debug.Log($"Ground: {IsGrounded}");
        Debug.Log($"Jump: {jump}");

        if (IsGrounded && jump)
        {
            Debug.Log(IsGrounded);

            Move(JumpForce, Vector3.up, ForceMode.Impulse);
        }
    }

    void Move(float force, Vector3 direction, ForceMode mode)
    {
        _playerRb.AddForce(ForceModifier * force * direction.normalized, mode);
    }

}
