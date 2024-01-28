using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

using System.Collections;
using System.Collections.Generic;
using FishNet.Component.Animating;
using FishNet.Object;

public class PlayerController : NetworkBehaviour
{
    public Rigidbody _playerRb;

    [SerializeField] public GameObject playerModel;

    [SerializeField] public Transform orientation;
    
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public InputAction moveAction;

    [SerializeField] public InputActionReference attack;
    public bool isAttacking;
    
    [SerializeField] private float dragCoef;

    public bool blue, red;
    public NetworkAnimator _BlueNetworkAnimator, _RedNetworkAnimator;
    public GameObject blueModel, redModel;

    public UIHandler UIHandlerSC;


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
        {
            //Debug.Log("NOT THE OWNER 2 second start");
            //StartCoroutine(WaitForSeconds(2f));
            this.gameObject.GetComponent<PlayerController>().enabled = false;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (_playerRb == null)
        {
            _playerRb = GetComponent<Rigidbody>();
        }

        moveAction = playerInput.actions["Movement"];

        //Debug.Log("2 second start");
        //StartCoroutine(WaitForSeconds(2f));
    }

    
    //public IEnumerator WaitForSeconds(float seconds)
    //{
    //    yield return new WaitForSeconds(seconds);
    //    RPCPlayerSet();
    //}

    //[ServerRpc(RequireOwnership = false)]
    //private void RPCPlayerSet()
    //{
    //    SetPlayers();
    //}
    

    //[ObserversRpc]
    //private void SetPlayers()
    //{
    //    Debug.Log("SET PLAYERS CALLED");
    //    if (UIHandlerSC.player1)
    //    {
    //        Debug.Log("Turning on red");
    //        this.gameObject.GetComponent<PlayerController>().red = true;
    //        this.gameObject.GetComponent<PlayerController>().redModel.SetActive(true);
    //    }
    //    else if (UIHandlerSC.player2)
    //    {
    //        Debug.Log("Turning on blue");
    //        this.gameObject.GetComponent<PlayerController>().red = false;
    //        this.gameObject.GetComponent<PlayerController>().redModel.SetActive(false);

    //    }
    //}


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
    

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
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
            isAttacking = true;

            if (blue)
            {
                _BlueNetworkAnimator.SetTrigger("Attack");
            }
            if (red)
            {
                _RedNetworkAnimator.SetTrigger("Attack");
            }
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
