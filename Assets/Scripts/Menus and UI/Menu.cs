using UnityEngine;
//Abstract can't be used directly and must be inherited from other scripts
public abstract class Menu : MonoBehaviour
{
    //protected = only my children can see this
    //virtual = this function can be overridden by children
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            Close();
        }
    }

    /// <summary>
    /// Close the menu by disabling the game object
    /// </summary>
    public void Close() 
    { 
        gameObject.SetActive(false);
    }
}
