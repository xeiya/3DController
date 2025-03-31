using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    //The point we grapple to
    private Vector3 grapplePoint;
    //Tells us what we can grapple on
    public LayerMask whatIsGrappable;

    public Transform gunTip, cam, player;
    private SpringJoint joint;

    private float maxDistance = 100f;

    public void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    public void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    public void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance))
        {
            grapplePoint = hit.point;
            //Adds a SpringJoint to our player
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point
            joint.maxDistance = distanceFromPoint * 0.7f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Spring is more pull and push with the grapple
            //Damper dampens the grapple
            //massScale refers to the mass of the grapple
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
        }
    }

    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    public void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    /// <summary>
    /// Draws a line from the gunTip to the grapplePoint
    /// </summary>
    public void DrawRope()
    {
        //Tells it if not grappling, don't draw rope
        if (!joint) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
