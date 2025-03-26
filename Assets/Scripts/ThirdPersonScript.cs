using UnityEngine;

public class ThirdPersonScript : MonoBehaviour
{
    [SerializeField] private float sensitivity = 1;
    
    [SerializeField] private float verticalRotationMin = -45;
    [SerializeField] private float verticalRotationMax = 70;

    [SerializeField] private Transform playerTransform;

    [Tooltip("The distance along which the camera will try to stay from the player, unless a wall is in the way")]
    [SerializeField] private float cameraZoomIdeal;

    [SerializeField] private LayerMask blockingLayer;

    //This is for rotating up/down
    private Transform pivotTransform;

    //This is for zooming in and out (On z axis)
    private Transform cameraTransform;

    private float currentHorizontalRotation;
    private float currentVerticalRotation;

    //The actual z position of the camera
    private float cameraZoomCurrent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set our starting rotation values
        currentHorizontalRotation = transform.localEulerAngles.y;
        currentVerticalRotation = transform.localEulerAngles.x;

        //Get a referance to the pivot (the first child object)
        pivotTransform = transform.GetChild(0);

        //Get a referance to the camera transform
        cameraTransform = pivotTransform.GetChild(0);

        //initialise our camera position
        cameraTransform.localPosition = new Vector3(0, 0, -cameraZoomIdeal);
    }

    // Update is called once per frame
    void Update()
    {
        //Update our desired rotation using inputs
        currentHorizontalRotation += Input.GetAxis("Mouse X") * sensitivity;
        currentVerticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;

        //Apply the rotation, clamping where required
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, verticalRotationMin, verticalRotationMax);
        transform.localEulerAngles = new Vector3(0, currentHorizontalRotation); //.z will be 0
        pivotTransform.localEulerAngles = new Vector3(currentVerticalRotation, 0);

        //Snap to the player's position
        transform.position = playerTransform.position;

        //Get the direction from the anchor point to the camera
        Vector3 direction = cameraTransform.position - transform.position;

        //if we hit something between the player and the camera..
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, cameraZoomIdeal, blockingLayer))
        {
            //adjust our zoom based on where we hit
            //hit.distamce is the distance between the raycast origion and the hit
            cameraZoomCurrent = hit.distance;
        }
        else 
        {
            //else, go to our ideal distance
            cameraZoomCurrent = cameraZoomIdeal;
        }

        //adjust the camera's z position to make it zoom in or out
        cameraTransform.localPosition = new Vector3(0, 0, -cameraZoomCurrent);
    }
}
