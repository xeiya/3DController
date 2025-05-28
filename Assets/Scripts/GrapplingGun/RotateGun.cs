using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun grappling;

    void Update()
    {
        //Rotates the gun based on where the grapple point is
        if (!grappling.IsGrappling()) return;
        transform.LookAt(grappling.GetGrapplePoint());
    }
}
