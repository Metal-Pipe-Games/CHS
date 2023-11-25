using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerIKController : MonoBehaviour
{
    public LayerMask layers;
    public float maxDistance = 50;

    public bool active = false;
    [Header("Left Hands")]
    public Transform TopLeftHandTarget;
    public Transform BottomLeftHandTarget;
    [Header("Right Hands")]
    public Transform TopRightHandTarget;
    public Transform BottomRightHandTarget;
    [Header("Legs")]
    public Transform RightFoot;
    public Transform LeftFoot;
    [Header("LegOffset")]
    public Vector3 position = Vector3.zero;
    public Vector3 rotation = Vector3.zero;
    [Header("Pointers")]
    [Header("Top Left Pointer")]
    public Transform TopLeftArm;
    public Transform TopLeftPointer;
    [Header("Bottom Left Pointer")]
    public Transform BottomLeftArm;
    public Transform BottomLeftPointer;
    [Header("Top Right Pointer")]
    public Transform TopRightArm;
    public Transform TopRightPointer;
    [Header("Bottom Right Pointer")]
    public Transform BottomRightArm;
    public Transform BottomRightPointer;
    [Header("Left Leg")]
    public Transform LeftLeg;
    public Transform LeftLegPointer;
    [Header("Right Leg")]
    public Transform RightLeg;
    public Transform RightLegPointer;

    Vector3 lastPos;

    private void Start()
    {
        lastPos = transform.position;
    }

    public void UpdateTargetHand(Transform target, Transform pointer, Transform root)
    {
        if (!active) return;
        Vector3 movement = transform.position - lastPos;
        Vector3 up = pointer.transform.position - root.transform.position;
        if (Physics.Raycast(root.position, up, out RaycastHit hit, maxDistance, layers.value))
        {
            Debug.DrawLine(root.position, pointer.transform.position + movement);
            target.transform.position = hit.point;

            Vector3 normal = hit.collider.transform.rotation * hit.normal;
            Vector3 targetForward = up - Vector3.Dot(up, normal) * normal;

            target.transform.rotation = Quaternion.LookRotation(targetForward, normal);
        }
    }

    public void UpdateTargetFoot(Transform target, Transform pointer, Transform root)
    {
        if (!active) return;
        Vector3 movement = transform.position - lastPos;
        Vector3 up = pointer.transform.position - root.transform.position;
        if (Physics.Raycast(pointer.position, up, out RaycastHit hit, maxDistance, layers.value))
        {
            target.transform.position = hit.point;

            Vector3 normal = hit.collider.transform.rotation * hit.normal;
            Quaternion normalRotation = Quaternion.LookRotation(normal);
            target.transform.position += normalRotation * position;
            Vector3 targetForward = up - Vector3.Dot(up, normal) * normal;

            target.transform.rotation = Quaternion.LookRotation(targetForward, normal);
            Vector3 euler = target.transform.rotation.eulerAngles;
            target.transform.rotation = Quaternion.Euler(
                euler.x + rotation.x,
                euler.y + rotation.y,
                euler.z + rotation.z
                );
        }
    }

    public void GetNewTopLeftHandTarget()
    {
        UpdateTargetHand(TopLeftHandTarget, TopLeftPointer, TopLeftArm);
    }
    public void GetNewBottomLeftHandTarget()
    {
        UpdateTargetHand(BottomLeftHandTarget, BottomLeftPointer, BottomLeftArm);
    }
    public void GetNewTopRightHandTarget()
    {
        UpdateTargetHand(TopRightHandTarget, TopRightPointer, TopRightArm);
    }
    public void GetNewBottomRightTarget()
    {
        UpdateTargetHand(BottomRightHandTarget, BottomRightPointer, BottomRightArm);
    }
    public void GetNewLeftLegTarget()
    {
        UpdateTargetHand(LeftFoot, LeftLegPointer, LeftLeg);
    }
    public void GetNewRightLegTarget()
    {
        UpdateTargetHand(RightFoot, RightLegPointer, RightLeg);
    }

    public void UpdateLastPos()
    {
        lastPos = transform.position;
    }
}
