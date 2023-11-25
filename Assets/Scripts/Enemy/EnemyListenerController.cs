using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListenerController : MonoBehaviour
{
    public GameObject parent;
    public bool startWallClimb = false;
    public bool wallClimbing = false;
    [Range(0,1.5f)]
    public float hitWidth = 0.1f;
    public float maxDistance = 10;
    public float wallWalkDistance = 0.5f;
    public float wallDistance = 0.4f;
    public float speed = 1f;
    public float rotation = 60f;

    public LayerMask layers;

    public Vector3 moveDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startWallClimb)
        {
            bool r = Physics.Raycast(transform.position, transform.right, out RaycastHit rHit, maxDistance);
            bool l = Physics.Raycast(transform.position, -transform.right, out RaycastHit lHit, maxDistance);

            RaycastHit? hit = null;
            if (r && l)
            {
                hit = (rHit.distance < lHit.distance) ? rHit : lHit;
            }
            else if (r) hit = rHit;
            else if (l) hit = lHit;

            if (hit != null)
            {
                Debug.DrawRay(transform.position, transform.right, Color.blue, rHit.distance);
                Debug.DrawRay(transform.position, -transform.right, Color.red, lHit.distance);
                moveDirection = r ? transform.right : -transform.right;
                if(hit.Value.distance < wallWalkDistance)
                {
                    wallClimbing = true;
                }
            }
            transform.position += speed * Time.deltaTime * moveDirection;
        }
        if (wallClimbing)
        {
            bool r = Physics.Raycast(transform.position + transform.right * hitWidth, transform.forward + transform.right * hitWidth, out RaycastHit rHit, maxDistance, layers.value);
            bool l = Physics.Raycast(transform.position + -transform.right * hitWidth, transform.forward - transform.right * hitWidth, out RaycastHit lHit, maxDistance, layers.value);

            if (r && l)
            {
                Debug.DrawRay(transform.position + transform.right * hitWidth, transform.forward + transform.right * hitWidth, Color.green, rHit.distance);
                Debug.DrawRay(transform.position + -transform.right * hitWidth, transform.forward - transform.right * hitWidth, Color.magenta, lHit.distance);
                Vector3 position = (rHit.point + lHit.point) * 0.5f + -transform.forward * wallDistance;
                Vector3 direction = (lHit.normal + rHit.normal)*0.5f;

                float angle = Vector3.Angle(direction, -transform.forward);
                if(angle > 0)Debug.Log(angle);

                transform.position = position;
                transform.RotateAround(transform.position, parent.transform.forward, angle*0.1f);
            }
        }
    }
}
