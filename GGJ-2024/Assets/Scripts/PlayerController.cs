using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRb;

    [SerializeField] public Transform orientation;

    [SerializeField] public Camera playerCamera;

    [SerializeField] LayerMask floorMask;

    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public InputAction moveAction;
    [SerializeField] public InputActionReference jumpAction;

    [SerializeField] public bool isMoving;
    [SerializeField] public bool isGrounded;

    [SerializeField] public float dragCoef;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (_playerRb == null)
        {
            _playerRb = GetComponent<Rigidbody>();
        }
        dragCoef = _playerRb.drag;
        
        moveAction = playerInput.actions["Movement"];
    }

    [SerializeField] private Vector2 mouseSens;
    private Vector2 _rotation;
    
    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSens.x;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSens.y;

        _rotation.x -= mouseY;
        _rotation.y += mouseX;

        _rotation.x = Mathf.Clamp(_rotation.x, -90f, 90f);

        playerCamera.transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        orientation.rotation = Quaternion.Euler(0, _rotation.y, 0);
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Jump();

        isGrounded = GroundCheck();

        _playerRb.drag = isGrounded ? dragCoef : 0f;
    }

    RaycastHit _raycastHit;

    private bool GroundCheck()
    {
        return Physics.Raycast(gameObject.transform.position, Vector3.down, out _raycastHit, 2f * 0.5f + 0.2f,
            floorMask);
    }

    [SerializeField] public float forceModifier = 5f;
    [SerializeField] public float moveSpeed = 5f;

    public void MovePlayer()
    {
        Vector2 movement = moveAction.ReadValue<Vector2>();

        if (movement != Vector2.zero)
        {
            isMoving = true;

            float movHor = movement.x;
            float movVert = movement.y;

            Vector3 inputDir = orientation.forward * movVert + orientation.right * movHor;

            Move(moveSpeed, inputDir, ForceMode.Force);
        }
        else
        {
            isMoving = false;
        }
    }

    [SerializeField] public float jumpForce = 5f;

    public void Jump()
    {
        bool jump = jumpAction.action.triggered;

        if (isGrounded && jump)
        {
            Move(jumpForce, Vector3.up, ForceMode.Impulse);
        }
    }

    void Move(float force, Vector3 direction, ForceMode mode)
    {
        _playerRb.AddForce(forceModifier * force * direction.normalized, mode);
    }
}
