using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun grappling;

    void Update()
    {
        if (!grappling.IsGrappling()) return;
        transform.LookAt(grappling.GetGrapplePoint());
    }
}
