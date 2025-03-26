using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        transform.LookAt(target);
    }
}
