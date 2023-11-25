using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class EnemyListenerAnimator : MonoBehaviour
{
    protected Animator animator;

    public bool isCrawling = false;
    [Range(0,1)]
    public float leftHandWeight = 1;
    [Range(0, 1)]
    public float rightHandWeight = 1;
    [Range(0, 1)]
    public float leftFootWeight = 1;
    [Range(0, 1)]
    public float rightFootWeight = 1;

    public Transform targetPos = null;

    public Transform rightHandSource = null;
    public Transform rightFootSource = null;
    public Transform leftHandSource = null;
    public Transform leftFootSource = null;

    public Transform rightHand = null;
    public Transform rightFoot = null;
    public Transform leftHand = null;
    public Transform leftFoot = null;

    public Transform rightArmStart = null;
    public Transform rightLegStart = null;
    public Transform leftArmStart = null;
    public Transform leftLegStart = null;

    private Transform rightHandTarget = null;
    private Transform rightFootTarget = null;
    private Transform leftHandTarget = null;
    private Transform leftFootTarget = null;

    public float handThreshold = 1;
    public float handDistance = 0.01f;
    public float footThreshold = 1;
    public float feetDistance = 0.01f;

    public LayerMask hitLayers;

    Vector3 lastPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Create global target Transforms.
        rightHandTarget = new GameObject(gameObject.name + "_rightHandTarget").GetComponent<Transform>();
        rightHandTarget.position = rightHand.position;

        leftHandTarget = new GameObject(gameObject.name + "_leftHandTarget").GetComponent<Transform>();
        leftHandTarget.position = leftHand.position;

        rightFootTarget = new GameObject(gameObject.name + "_rightFootTarget").GetComponent<Transform>();
        rightFootTarget.position = rightFoot.position;

        leftFootTarget = new GameObject(gameObject.name + "_leftFootTarget").GetComponent<Transform>();
        leftFootTarget.position = leftFoot.position;/**/

        lastPos = gameObject.transform.position;
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            //if the IK is active, set the position and rotation directly to the goal.
            if (isCrawling)
            {
                lastPos = gameObject.transform.position;
                // Set the look target position, if one has been assigned
                if (targetPos != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(targetPos.position);
                }

                if (leftHand != null)
                {
                    float weight = Mathf.Max(0, handThreshold - Vector3.Distance(leftHandSource.position, leftHandTarget.position)) * leftHandWeight;

                    if (Vector3.Distance(leftFootSource.position, leftFootTarget.position) >= footThreshold) 
                        CalculateLeftHandPoint();

                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightHand != null)
                {
                    float weight = Mathf.Max(0, handThreshold - Vector3.Distance(rightHandSource.position, rightHandTarget.position)) * rightHandWeight;

                    if (Vector3.Distance(rightHandSource.position, rightHandTarget.position) >= handThreshold)
                        CalculateRightHandPoint();

                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
                }

                if (leftFoot != null)
                {
                    float weight = Mathf.Max(0, footThreshold - Vector3.Distance(leftFootSource.position, leftFootTarget.position)) * leftFootWeight;

                    if (Vector3.Distance(leftFootSource.position, leftFootTarget.position) >= footThreshold)
                        CalculateLeftFootPoint();

                    animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weight);
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);
                }

                // Set the right hand target position and rotation, if one has been assigned
                if (rightFoot != null)
                {
                    float weight = Mathf.Max(0, footThreshold - Vector3.Distance(rightFootSource.position, rightFootTarget.position)) * rightFootWeight;


                    if (Vector3.Distance(rightFootSource.position, rightFootTarget.position) >= footThreshold)
                        CalculateRightFootPoint();

                    animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weight);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.position);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
                }

            }
        }
    }

    public void CalculateLeftHandPoint()
    {
        Vector3 red = (leftHandSource.position - leftArmStart.position);
        if (Physics.Raycast(leftArmStart.position, red, out RaycastHit fHit, handThreshold * 2, hitLayers.value))
        {
            leftHandTarget.position = fHit.point + fHit.normal * handDistance;

            Vector3 green = red - Vector3.Dot(red, fHit.normal) * fHit.normal;
            leftHandTarget.forward = green;

            if (Physics.Raycast(leftArmStart.position, (leftHandSource.position + leftHandSource.right * 0.05f - leftArmStart.position), out RaycastHit sHit, handThreshold * 2, hitLayers.value))
            {
                float angle = Vector3.Angle(leftHandTarget.right, sHit.point - fHit.point);

                leftHandTarget.RotateAround(leftHandTarget.position, leftHandTarget.forward, angle);
            }
        }
    }

    public void CalculateRightHandPoint()
    {
        Vector3 red = (rightHandSource.position - rightArmStart.position);
        if (Physics.Raycast(rightArmStart.position, red, out RaycastHit fHit, handThreshold * 2, hitLayers.value))
        {
            rightHandTarget.position = fHit.point + fHit.normal * handDistance;

            Vector3 green = red - Vector3.Dot(red, fHit.normal) * fHit.normal;
            rightHandTarget.forward = green;


            if (Physics.Raycast(rightArmStart.position, (rightHandSource.position + rightHandSource.right * 0.05f - rightArmStart.position), out RaycastHit sHit, handThreshold * 2, hitLayers.value))
            {
                float angle = Vector3.Angle(rightHandTarget.right, sHit.point - fHit.point);

                rightHandTarget.RotateAround(rightHandTarget.position, rightHandTarget.forward, angle);
            }
        }
    }

    public void CalculateLeftFootPoint()
    {
        Vector3 red = (leftFootSource.position - leftLegStart.position);
        if (Physics.Raycast(leftLegStart.position, red, out RaycastHit fHit, footThreshold * 2, hitLayers.value))
        {
            leftFootTarget.position = fHit.point + fHit.normal * feetDistance;

            Vector3 green = red - Vector3.Dot(red, fHit.normal) * fHit.normal;
            leftFootTarget.forward = green;


            if (Physics.Raycast(leftLegStart.position, (leftFootSource.position + leftFootSource.right * 0.01f - leftLegStart.position), out RaycastHit sHit, footThreshold * 2, hitLayers.value))
            {
                float angle = Vector3.Angle(leftFootTarget.right, sHit.point - fHit.point);

                leftFootTarget.RotateAround(leftFootTarget.position, leftFootTarget.up, angle);
            }
        }
    }

    public void CalculateRightFootPoint()
    {
        Vector3 red = (rightFootSource.position - rightLegStart.position);
        if (Physics.Raycast(rightLegStart.position, red, out RaycastHit fHit, footThreshold * 2, hitLayers.value))
        {
            rightFootTarget.position = fHit.point + fHit.normal * feetDistance;

            Vector3 green = red - Vector3.Dot(red, fHit.normal) * fHit.normal;
            rightFootTarget.forward = green;


            if (Physics.Raycast(rightLegStart.position, (rightFootSource.position + rightFootSource.right * 0.01f - rightLegStart.position), out RaycastHit sHit, footThreshold * 2, hitLayers.value))
            {
                float angle = Vector3.Angle(rightFootTarget.right, sHit.point - fHit.point);

                rightFootTarget.RotateAround(rightFootTarget.position, rightFootTarget.up, angle);
            }
        }
    }
}
