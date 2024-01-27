using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

using FishNet.Component.Animating;
using FishNet.Object;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody _playerRb;

    [SerializeField] public GameObject playerModel;

    [SerializeField] public Transform orientation;
    
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public InputAction moveAction;

    [SerializeField] public InputActionReference attack;
    
    [SerializeField] private float dragCoef;


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            this.gameObject.GetComponent<PlayerController>().enabled = false;
            return;
        }

        _networkAnimator = GameObject.FindWithTag("Model").GetComponent<NetworkAnimator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (_playerRb == null)
        {
            _playerRb = GetComponent<Rigidbody>();
        }

        moveAction = playerInput.actions["Movement"];
    }


    //    // Start is called before the first frame update
    //    void Start()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
        
    //    if (_playerRb == null)
    //    {
    //        _playerRb = GetComponent<Rigidbody>();
    //    }
        
    //    moveAction = playerInput.actions["Movement"];
    //}


    [SerializeField] private Camera playerCamera;
    [SerializeField] private Vector2 mouseSens;
    private Vector2 _rotation;
    public NetworkAnimator _networkAnimator;

    private void Update()
    {
        // TODO: Change to gamepad joystick.
        var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSens.x;
        var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSens.y;

        _rotation.x -= mouseY;
        _rotation.y += mouseX;

        _rotation.x = Mathf.Clamp(_rotation.x, -90f, 90f);

        // TODO: Remove camera rotation.
        //playerCamera.transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        
        //orientation.rotation = Quaternion.Euler(0, _rotation.y, 0);

        if (attack.action.triggered)
        {
            _networkAnimator.SetTrigger("Attack");
        }
    }

    [SerializeField] private bool isGrounded;
    private void FixedUpdate()
    {
        MovePlayer();

        if (jumpAction.action.triggered)
        {
            Jump();
        }
        
        isGrounded = GroundCheck();

        //_playerRb.drag = isGrounded ? dragCoef : 0f;
    }

    RaycastHit _raycastHit;
    [SerializeField] LayerMask floorMask;
    private bool GroundCheck()
    {
        return Physics.Raycast(gameObject.transform.position, 
                            Vector3.down, out _raycastHit, 
                            2f * 0.5f + 0.2f,
                                floorMask);
    }

    [SerializeField] private float forceModifier = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public bool isMoving;
    public void MovePlayer()
    {
        var movement = moveAction.ReadValue<Vector2>();

        if (movement != Vector2.zero)
        {
            isMoving = true;

            _playerRb.constraints = ~RigidbodyConstraints.FreezePosition;
            _playerRb.constraints = RigidbodyConstraints.FreezeRotationY;

            var movHor = movement.x;
            var movVert = movement.y;

            var inputDir = orientation.forward * movVert + orientation.right * movHor;

            Move(moveSpeed, inputDir, ForceMode.Force);
        }
        else
        {
            isMoving = false;
            _playerRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float airMultiplier = 5f;
    [SerializeField] private bool readyToJump = true;
    public void Jump()
    {
        if (isGrounded && readyToJump)
        {
            Move(jumpForce, Vector3.up, ForceMode.Impulse);
            readyToJump = false;
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    void Move(float force, Vector3 direction, ForceMode mode)
    {
        if (isGrounded)
        {
            _playerRb.AddForce(direction.normalized * (forceModifier * force) , mode);
        }
        else
        {
            _playerRb.AddForce(direction.normalized * (forceModifier * force * airMultiplier), mode);
        }

        playerModel.transform.forward = direction.normalized;
    }
}
