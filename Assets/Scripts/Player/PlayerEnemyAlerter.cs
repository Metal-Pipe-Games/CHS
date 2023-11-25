using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyAlerter : MonoBehaviour
{
    public List<GameObject> enemies = new();
    public float WalkStrength = .5f;
    public float LandStrength = 2;
    public Vector3? aggroPos = null;
    public LayerMask layerMask;
    public float maxDistance = 50;
    private float maxmul;
    // Start is called before the first frame update
    void Start()
    {
        maxmul = 1 / maxDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Alert(int type)
    {
        float strength = 0;
        switch (type)
        {
            case 0:
                strength = WalkStrength;
                break;
            case 1:
                strength = LandStrength;
                break;
            case 2:
                strength = WalkStrength * 2;
                break;
        }

        enemies.RemoveAll(value=>value==null);
        foreach (var enemy in enemies)
        {
            if (Physics.Raycast(transform.position, enemy.transform.position - transform.position, out RaycastHit hit, maxDistance, layerMask.value))
            {
                if (hit.collider.CompareTag("Enemy")) 
                    enemy.transform.root.GetComponent<EnemyController>().Alert(
                        strength * (1 - Vector3.Distance(transform.position, 
                        enemy.transform.position) * maxmul), 
                        gameObject.transform.position);
            }
            else
            {
                //Debug.Log(hit.collider.gameObject.tag);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
        }
    }
}