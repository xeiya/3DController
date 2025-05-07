using UnityEngine;


//Makes sure the game object has these components attached
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CameraSelector))]
public class StateMachineMovement : MonoBehaviour
{
    //This enumerator is for our possible states
    public enum State
    {
        Walk,
        Rise,
        Fall,
        Jet,
        Grappling
    }

    public GrapplingGun grappling;

    [SerializeField] private State stateCurrent;

    [SerializeField] private float speedWalk;

    [SerializeField] private float jumpPower;

    [SerializeField] private float gravityUp, gravityDown;

    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;

    private CapsuleCollider capsuleCollider;

    private CameraSelector cameraSelector;

    //how many jumps total we're allowed
    private int jumpsAllowed = 2;

    //how many jumps we have at the moment
    private int jumpsRemaining;

    [SerializeField] private float jetFuelMax;
    [SerializeField] private float jetAccel;
    [SerializeField] private float jetRefuelSpeed;

    private float jetFuelCurrent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get our components
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        cameraSelector = GetComponent<CameraSelector>();

        //make sure our rigidbody is set up correctly
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = false;

        //start with the max number of jumps
        jumpsRemaining = jumpsAllowed;
        //start with the max amount of jet fuel
        jetFuelCurrent = jetFuelMax;
    }

    // Update is called once per frame
    void Update()
    {
        //Depending on the current state, choose from a set of behaviour to follow
        switch (stateCurrent)
        {
            case State.Walk:
                WalkState();
                break;
            case State.Rise:
                RiseState();
                break;
            case State.Fall:
                FallState();
                break;
            case State.Jet:
                JetState();
                break;
            case State.Grappling:
                break;
        }
    }

    private void WalkState()
    {
        //Reset our double jump
        jumpsRemaining = jumpsAllowed;

        //Refill our jet fuel over time (the higher jetRefuelSpeed is, the faster it will refuel)
        jetFuelCurrent = Mathf.MoveTowards(jetFuelCurrent, jetFuelMax, jetRefuelSpeed * Time.deltaTime);

        //get our input direction in local space
        Vector3 inputMovement = GetMovementFromInput();

        //increase that using our base walk speed
        inputMovement *= speedWalk; //same as "inputMovement = inputMovement * speedWalk"

        //make sure we're on the ground, but not building up vertical speed
        inputMovement.y = Mathf.Clamp(rb.linearVelocity.y - gravityDown * Time.deltaTime, 0f, float.PositiveInfinity);

        //apply the movement to our rigidbody
        rb.linearVelocity = inputMovement;

        //Check if we're supposed to be in a different state
        if (!IsGrounded())
        {
            //we should be falling
            stateCurrent = State.Fall;

            //the player can only jump once out of a fall
            jumpsRemaining -= 1;
        }
        else 
        {
            if (Input.GetButton("Jump")) 
            {
                //go to our rise state
                RiseAtSpeed(jumpPower);
                jumpsRemaining -= 1;
            }
        }

    }

    private void RiseState() 
    {
        Vector3 inputMovement = GetMovementFromInput();
        inputMovement *= speedWalk;

        //we are rising. so use our "up" gravity
        inputMovement.y = rb.linearVelocity.y - gravityUp * Time.deltaTime;

        rb.linearVelocity = inputMovement;

        //if linearVelocity.y is less that 0, we have started to fall
        if (rb.linearVelocity.y < 0f) 
        {
            stateCurrent = State.Fall;
        }
        //check for a double jump
        if (jumpsRemaining > 0 && Input.GetButtonDown("Jump")) 
        {
            RiseAtSpeed(jumpPower);
            jumpsRemaining -= 1;
        }

        //if we're holding jump AND have fuel remaining
        if (Input.GetButton("Jump") && jetFuelCurrent > 0)
        {
            stateCurrent = State.Jet;
        }
    }

    private void FallState() 
    {
        Vector3 inputMovement = GetMovementFromInput();
        inputMovement *= speedWalk;

        //we are rising. so use our "up" gravity
        inputMovement.y = rb.linearVelocity.y - gravityDown * Time.deltaTime;

        rb.linearVelocity = inputMovement;

        //if we are on the ground...
        if (IsGrounded()) 
        {
            //state should be changed to walk
            stateCurrent = State.Walk;
        }
        //check for a double jump
        if (jumpsRemaining > 0 && Input.GetButtonDown("Jump"))
        {
            RiseAtSpeed(jumpPower);
            jumpsRemaining -= 1;
        }

        //if we're holding jump AND have fuel remaining
        if (Input.GetButton("Jump") && jetFuelCurrent > 0)
        {
            stateCurrent = State.Jet;
        }
    }

    private void JetState()
    {
        jetFuelCurrent -= Time.deltaTime;

        Vector3 inputMovement = GetMovementFromInput();

        inputMovement *= speedWalk;

        //accel upwards getting faster over time
        inputMovement.y = rb.linearVelocity.y + jetAccel * Time.deltaTime;

        rb.linearVelocity = inputMovement;

        //if the player lets go of jump, OR they run out of fuel
                                    // "||" means "or", aka only one of these things need to be true
        if (Input.GetButtonUp("Jump") || jetFuelCurrent <= 0) 
        {
            stateCurrent = State.Rise;
        }
    }

    public void SetState(State newState) 
    {
        stateCurrent = newState;
    }

    private void RiseAtSpeed(float speed) 
    {
        //give our rigidbody upward speed
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, speed, rb.linearVelocity.z);

        stateCurrent = State.Rise;
    }

    private Vector3 GetMovementFromInput() 
    {
        Vector2 inputThisFrame = new Vector2();
        inputThisFrame.x = Input.GetAxis("Horizontal");
        inputThisFrame.y = Input.GetAxis("Vertical");


        Vector3 moveDirection = new Vector3(inputThisFrame.x, 0, inputThisFrame.y);

        if (cameraSelector.GetCurrentSelection() == CameraSelector.Selection.FirstPerson)
        {
            //rotate the player left/right to match the camera
            transform.localEulerAngles = new Vector3(0, cameraSelector.GetCameraTransform().localEulerAngles.y);
            //translate the direction from world to local
            moveDirection = transform.TransformDirection(moveDirection);
        }
        else 
        {
            moveDirection = cameraSelector.GetCameraTransform().TransformDirection(moveDirection);
        }

        return moveDirection;
    }

   
    private bool IsGrounded() 
    {
        return Physics.Raycast(transform.position, Vector3.down, capsuleCollider.height / 2f + 0.01f, groundMask);
    }
}

