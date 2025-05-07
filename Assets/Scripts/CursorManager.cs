using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetCursor(bool isFree) 
    {
        Cursor.visible = isFree;

        if (isFree)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else 
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Ternary operator - use the first value if 'isFree' is true, the second if false
        Cursor.lockState = isFree ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
