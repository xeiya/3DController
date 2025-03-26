using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraSelector : MonoBehaviour
{
    public enum Selection 
    { 
        FirstPerson,
        ThirdPerson
    }

    [SerializeField] private Selection selection;

    private FirstPersonCamera firstPersonCamera;

    private ThirdPersonScript thirdPesonCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //FindFirstObjecetByType is like GetComponent, but it checks the whole scene
        firstPersonCamera = FindFirstObjectByType<FirstPersonCamera>();
        thirdPesonCamera = FindAnyObjectByType<ThirdPersonScript>();

        //Set up the correct starting camera
        SelectCamera(selection);
    }

    /// <summary>
    /// Change the current camera selection to a new selection
    /// </summary>
    /// <param name="newSelection"></param>
    public void SelectCamera(Selection newSelection) 
    {
        selection = newSelection;

        switch (selection)
        {
            case Selection.FirstPerson:
                thirdPesonCamera.gameObject.SetActive(false);
                firstPersonCamera.gameObject.SetActive(true);
                break;
            case Selection.ThirdPerson:
                firstPersonCamera.gameObject.SetActive(false);
                thirdPesonCamera.gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Returns the current selection type
    /// </summary>
    /// <returns></returns>
    public Selection GetCurrentSelection() 
    {
        return selection;
    }

    /// <summary>
    /// Returns the transform of the currently active camera
    /// </summary>
    /// <returns></returns>
    public Transform GetCameraTransform() 
    {
        switch (selection)
        {
            case Selection.FirstPerson:
                return firstPersonCamera.transform;
            case Selection.ThirdPerson:
                return thirdPesonCamera.transform;
            default:
                return null; //null means nothing
        }
    }
}
