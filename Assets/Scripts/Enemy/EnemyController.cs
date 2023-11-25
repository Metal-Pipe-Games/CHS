using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Animator animator;

    public float aggro = 0;
    public float cAggro = 0;
    public float searchThreshhold = 3;
    private bool searching = false;
    public float threshhold = 10;
    public float maxThreshhold = 30;
    public float positionMaxThreshhold = 10;

    public float calmness = 2f;
    public float hearing = 1;
    public float sight = 1;
    public float sensitivity = 1;

    public Vector3 target;
    public float maxMergeDistance = 1;
    private int targetCount = 0;
    public Dictionary<Vector3, float> targets = new();
    public float attackRadius = 5;
    private NavMeshAgent agent;
    private float threshmult;

    public bool alive = true;
    public int enemyId;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.stoppingDistance = attackRadius;
        threshmult = 1 / threshhold;
    }

    // Update is called once per frame
    void Update()
    {
        Pair strongestPair = new(transform.position,0);
        List<KeyValuePair<Vector3, float>> cTargets = new(targets);
        var calmnessRem = calmness * Time.deltaTime;
        foreach (KeyValuePair<Vector3,float> pair in cTargets)
        {
            float kAggro = pair.Value - calmnessRem;

            if (kAggro < 0)
            {
                targets.Remove(pair.Key);
                targetCount--;
                continue;
            }
            if (kAggro > positionMaxThreshhold)
            {
                kAggro = positionMaxThreshhold;
            }
            targets[pair.Key] = kAggro;
            if(kAggro >= strongestPair.value)
            {
                strongestPair.key = pair.Key;
                strongestPair.value = pair.Value;
            }
        }
        target = strongestPair.key;

        if (aggro > 0)
        {
            aggro -= calmness * Time.deltaTime;
        }
        if (aggro > maxThreshhold)
        {
            aggro = maxThreshhold;
        }
        if(aggro < 0)
        {
            aggro = 0;
        }
        float cAggro = aggro + strongestPair.value;
        this.cAggro = cAggro;

        if (cAggro > maxThreshhold)
        {
            cAggro = maxThreshhold;
        }
        else if (cAggro < 0)
        {
            cAggro = 0;
        }
        //Debug.Log(aggro);
        if (cAggro >= threshhold)
        {
            agent.stoppingDistance = attackRadius;
            if (Vector3.Distance(gameObject.transform.position, target) <= attackRadius)
            {
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                animator.SetTrigger("Attacking");
            }
            else
            {
                if (agent.isStopped) agent.isStopped = false;
                agent.destination = target;

                if(cAggro < threshhold) agent.destination = agent.steeringTarget;
            }
            if (!agent.isStopped)
            {
                transform.position = Vector3.MoveTowards(transform.position, agent.nextPosition, Time.deltaTime * agent.speed);
            }
        }else if (threshhold >= searchThreshhold && !searching)
        {
            searching = true;
            if (Vector3.Distance(gameObject.transform.position, target) <= attackRadius)
            {
                agent.velocity = Vector3.zero;
                agent.isStopped = true;
                animator.SetTrigger("Attacking");
            }
            agent.destination = target;
        }
    }

    public struct Pair
    {
        public Vector3 key;
        public float value;

        public Pair(Vector3 key, float value)
        {
            this.key = key;
            this.value = value;
        }
    }
    public void Alert(float strength,Vector3 target)
    {
        aggro += hearing * strength * (threshmult * Mathf.Min(aggro, threshhold) + .4f);
        if(targetCount > 0)
        {
            Vector3 closestKey = Vector3.zero;
            float closestKeyDist = Mathf.Infinity;
            foreach(Vector3 pos in targets.Keys)
            {
                float currDist = Vector3.Distance(pos, target);
                if (currDist < closestKeyDist)
                {
                    closestKeyDist = currDist;
                    closestKey = pos;
                }
            }
            if(closestKeyDist <= maxMergeDistance)
            {
                if (targets.TryGetValue(closestKey, out float lAggro))
                {
                    float cAggro = hearing * strength * (threshmult * Mathf.Min(aggro, threshhold) + .4f) + lAggro / (1 + closestKeyDist);
                    if (closestKey.Equals(target))
                    {
                        targets[closestKey] = cAggro;
                    }
                    else
                    {
                        targets.Add(target, cAggro);
                        targetCount++;

                        targets.Remove(closestKey);
                        targetCount--;
                    }
                }
                else
                {
                    float cAggro = hearing * strength * (threshmult * Mathf.Min(aggro, threshhold) + .4f);
                    targets.Add(target, cAggro);
                }
            }
            else
            {
                float cAggro = hearing * strength * (threshmult * Mathf.Min(aggro, threshhold) + .4f);
                targets.Add(target, cAggro);
                targetCount++;
            }
        }
        else
        {
            float cAggro = hearing * strength * (threshmult * Mathf.Min(aggro, threshhold) + .4f);
            targets.Add(target, cAggro);
            targetCount++;
        }
    }

    public void SetAttacking(bool b)
    {

    }
}
