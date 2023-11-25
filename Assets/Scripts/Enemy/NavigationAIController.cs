using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationAIController : MonoBehaviour
{
    public Transform targetTransform;
    Vector3 oldTransformPos = Vector3.zero;

    private NavMeshPath path;
    public NavMeshTarget[] targets;
    public NavMeshTarget target;
    public int attemptPaths = 10;
    public int maxCorners = 50;
    private int activeTargetId = 0;
    public bool canMove = true;
    public Vector3? targetPosition = null;
    float distanceToTarget = 0;
    float distanceDivition = 0;
    public float rotationDistance = 0.7f;
    float velocity = 0;
    public float Speed = 1;
    public float Accelleration = 0.2f;
    public float height = 1;
    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        if(targetTransform.position != oldTransformPos)
        {
            SetTarget(targetTransform.position);
            oldTransformPos = targetTransform.position;
        }

        if (targetPosition != null && canMove)
        {
            Vector3 bottom = transform.position - 0.5f * height * transform.up;

            target = targets[activeTargetId];

            float distance = Vector3.Distance(bottom, targets[activeTargetId].position);
            if(distance < 1)
            {
                if(activeTargetId < targets.Length)
                {
                    if(velocity > Speed * 0.5f)
                    {
                        velocity -= Time.deltaTime * Accelleration * 2;
                        if (velocity < Speed * 0.5f) velocity = Speed * 0.5f;
                    }
                }
                else
                {
                    if(activeTargetId < targets.Length - 1)
                    {
                        //Quaternion targetRotation = Quaternion.LookRotation(targets[activeTargetId + 1].position - target.position, targets[activeTargetId].normal);

                        //transform.rotation = targetRotation;
                    }
                    if (velocity > 0)
                    {
                        velocity -= Time.deltaTime * Accelleration * 2;
                        if (velocity < 0) velocity = 0;
                    }
                }
            }
            else
            {
                if(velocity < Speed)
                {
                    velocity += Time.deltaTime * Accelleration;
                    if (velocity > Speed) velocity = Speed;
                }
            }
            Vector3 goTo = Vector3.MoveTowards(bottom, targets[activeTargetId].position, velocity * Time.deltaTime);
            if(activeTargetId != 0)
            {
                float difference = Vector3.Distance(transform.position, targets[activeTargetId].position)/distanceDivition;
                Vector3 normal = Vector3.Lerp(targets[activeTargetId-1].normal, targets[activeTargetId].normal, difference);

                transform.up = normal;
                //Debug.Log(normal);
            }
            else
            {

            }

            //Debug.Log(bottom + "   " + targets[activeTargetId].position + "   " + (goTo - bottom));
            transform.position = goTo + 0.5f * height * transform.up;
            /*
            if (NavMesh.CalculatePath(goTo, targetPosition.Value, NavMesh.AllAreas, path))
            {
                activeTargetId = 0;
                targets = new NavMeshTarget[path.corners.Length];
                for (int i = 0; i < path.corners.Length; i++)
                {
                    var corner = path.corners[i];
                    if (NavMesh.SamplePosition(corner, out NavMeshHit lhit, 50, NavMesh.AllAreas))
                    {
                        targets[i] = new NavMeshTarget(corner, lhit.normal);
                    }
                }
            }
            */
            if (distance < 0.1) {
                activeTargetId++;
                if(activeTargetId < targets.Length)distanceToTarget = Vector3.Distance(targets[activeTargetId-1].position, targets[activeTargetId].position);
            }
            if (activeTargetId >= targets.Length) targetPosition = null;
        }
    }

    public void SetTarget(Vector3 target)
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit bottomHit, 50, NavMesh.AllAreas))
        {
            Vector3 bottom = bottomHit.position;
            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 50, NavMesh.AllAreas))
            {
                path = new();

                NavMeshPath[] paths = new NavMeshPath[attemptPaths];

                var tempCorners = new Vector3[maxCorners];
                float lowestCost = Mathf.Infinity;
                int lowestCostPathIndex = -1;
                for (int p = 0; p < attemptPaths; p++)
                {
                    paths[p] = new NavMeshPath();
                    if (NavMesh.CalculatePath(bottom, hit.position, NavMesh.AllAreas, paths[p]))
                    {
                        var tempPath = paths[p];

                        var c = tempPath.GetCornersNonAlloc(tempCorners);

                        if (c > maxCorners) continue;
                        float LastCost = 0;
                        float TotalCost = 0;
                        for(int i = 0; i < c; i++)
                        {
                            if (NavMesh.SamplePosition(tempCorners[i], out NavMeshHit cHit,50,NavMesh.AllAreas))
                            {
                                int mask = cHit.mask;
                                float areaCost = NavMesh.GetAreaCost(mask);
                                if(i == 0)
                                {
                                    LastCost = areaCost;
                                    continue;
                                }
                                float travelCost = Mathf.Min(areaCost,LastCost);
                                float distance = Vector3.Distance(tempCorners[i - 1], tempCorners[i]);
                                float cost = travelCost * distance;
                                Debug.DrawRay(tempCorners[i - 1], (tempCorners[i]- tempCorners[i-1]), Color.Lerp(Color.blue,Color.red,cost/30), 10);
                                
                                LastCost = areaCost;
                                TotalCost += cost;
                            }
                        }

                        if(TotalCost < lowestCost)
                        {
                            lowestCost = TotalCost;
                            lowestCostPathIndex = p;
                        }
                    }
                }
                if(lowestCostPathIndex != -1)
                {
                    path = paths[lowestCostPathIndex];
                    targetPosition = hit.position;
                    activeTargetId = 0;
                    targets = new NavMeshTarget[path.corners.Length];
                    for (int i = 0; i < path.corners.Length; i++)
                    {
                        var corner = path.corners[i];
                        if (NavMesh.SamplePosition(corner, out NavMeshHit lhit, 50, NavMesh.AllAreas))
                        {
                            var normal = lhit.normal;
                            var mask = lhit.mask;

                            Debug.Log(mask + "   " + NavMesh.GetAreaFromName("Roof"));
                            
                            targets[i] = new NavMeshTarget(corner, lhit.normal);
                            //Debug.Log(lhit.normal);
                        }
                    }
                    distanceToTarget = Vector3.Distance(transform.position, targets[0].position);
                    distanceDivition = 1 / (distanceToTarget > 0 ? distanceToTarget : 1);
                }
                /*
                if (NavMesh.CalculatePath(bottom, hit.position, NavMesh.AllAreas, path))
                {
                    targetPosition = hit.position;
                    activeTargetId = 0;
                    targets = new NavMeshTarget[path.corners.Length];
                    for (int i = 0; i < path.corners.Length; i++)
                    {
                        var corner = path.corners[i];
                        if (NavMesh.SamplePosition(corner, out NavMeshHit lhit, 50, NavMesh.AllAreas))
                        {
                            targets[i] = new NavMeshTarget(corner, lhit.normal);
                        }
                    }
                    distanceToTarget = Vector3.Distance(transform.position, targets[0].position);
                    distanceDivition = 1 / (distanceToTarget > 0 ? distanceToTarget:1);
                }
                else
                {
                    Debug.Log("Fail path");
                }/**/
            }
            else
            {
                Debug.Log("Fail hit");
            }
        }
        else
        {
            Debug.Log("Fail bottom");
        }
    }
}

public struct NavMeshTarget
{
    public Vector3 position;
    public Vector3 normal;

    public NavMeshTarget(Vector3 position, Vector3 normal)
    {
        this.position = position;
        this.normal = normal;
    }
}