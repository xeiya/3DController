using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Tooltip("How much the camera should mouse movement affect camera movement")]
    [SerializeField] private float sensitivity;

    [Tooltip("The transform of the player object")]
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float verticalRotationMin = -30;
    [SerializeField] private float verticalRotationMax = 60;

    [Tooltip("How far up the player model the camera should move")]
    [SerializeField] private float playerEyeOffSet = 0.5f;
    

    private float currentHorizontalRotation;
    private float currentVerticalRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHorizontalRotation = transform.localEulerAngles.y;
        currentVerticalRotation = transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        currentHorizontalRotation += Input.GetAxis("Mouse X") * sensitivity;

        //we use - here because screens start at 0 at the top and get bigger going down,
        //which is the opposite of Unity.
        currentVerticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;

        //Constrain the camera's up/down so we don't snap our neck
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, verticalRotationMin, verticalRotationMax);

        Vector3 newRotation = new Vector3();
        newRotation.x = currentVerticalRotation;
        newRotation.y = currentHorizontalRotation;

        transform.localEulerAngles = newRotation;

        //go to the player
        transform.position = playerTransform.position;
        //go up a bit to the player's eyes
        transform.position += Vector3.up * playerEyeOffSet;
    }
}
