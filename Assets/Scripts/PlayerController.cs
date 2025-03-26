using UnityEngine;

//Tells the game object - you must have a rigidbody
//if you don't have one... too bad!
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerController : MonoBehaviour
{
    //private = no one else can see this
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private CameraSelector cameraSelector;

    //serialize = make data readable, field = the variable
    [SerializeField] private float walkSpeed;

    [SerializeField] private float gravity;

    [SerializeField] private float jumpPower;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //store the rigidbody on this game object in rb
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        cameraSelector = GetComponent<CameraSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        //get a local (temporary) vector 2
        Vector2 inputThisFrame = new Vector2();
        
        //get the Horizontal/Vertical input
        inputThisFrame.x = Input.GetAxis("Horizontal");
        inputThisFrame.y = Input.GetAxis("Vertical");

        //Vector3 here because movement is in 3D
        Vector3 movement = new Vector3();
        movement.x = inputThisFrame.x * walkSpeed;
        movement.z = inputThisFrame.y * walkSpeed;

        //Transform our input direction based on which camera is looking where

        if (cameraSelector.GetCurrentSelection() == CameraSelector.Selection.FirstPerson) ///same as if (cameraTransform !=null) 
        {
            //rotate the player left or right based on the camera rotation
            transform.localEulerAngles = new Vector3(0, cameraSelector.GetCameraTransform().localEulerAngles.y, 0);
            movement = cameraSelector.GetCameraTransform().TransformDirection(movement);
        }
        else 
        { 
            movement = cameraSelector.GetCameraTransform().TransformDirection(movement);
        
        }

        //else if (thirdPersonCameraTransform) 
        //{
            //Turn movement to local space based on our camera's left/right rotation
            //movement = thirdPersonCameraTransform.TransformDirection(movement);
        //}


        //set the up/down movement based on the rigidbody's movement
        //gravity happens overtime so it is deltatime
        movement.y = rb.linearVelocity.y - gravity * Time.deltaTime;

        //apply the movement to our rigidbody
        rb.linearVelocity = movement;

        //check if we need to jump as well
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) 
        {
            Jump();
        }
    }

    private void Jump() 
    {
        //copy the rigidbody's movement
        Vector3 jumpMovement = rb.linearVelocity;

        //Replace the y value with out desired jump
        jumpMovement.y = jumpPower;

        //apply that movement back to our rigidbody
        rb.linearVelocity = jumpMovement;
    }

    private bool IsGrounded() 
    {
        //draw a line from our current position, downwards, half the height of the player.
        return Physics.Raycast(transform.position, Vector3.down, capsuleCollider.height / 2f + 0.01f);
    }
}
