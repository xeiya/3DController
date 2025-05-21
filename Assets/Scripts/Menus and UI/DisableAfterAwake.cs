using UnityEngine;

public class DisableAfterAwake : MonoBehaviour
{
    bool hasDisabled;

    private void Awake()
    {
        if (!hasDisabled) 
        { 
            hasDisabled = true;
            gameObject.SetActive(false);
        }
    }
}
