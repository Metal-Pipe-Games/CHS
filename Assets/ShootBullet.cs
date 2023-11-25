using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform target;
    public ToolHandler handler;
    float Attack;
    [Header("\nCasing")]
    public Transform casingFrom;
    public GameObject casingPrefab;
    public float startForce;
    private void Start()
    {
        Attack = handler.tools[Tools.Gun].AttackDamage;
    }
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = target.position;
        bullet.transform.forward = target.forward;
        bullet.GetComponent<BulletController>().Attack = Attack;
    }

    public void DropCasing()
    {
        GameObject casing = Instantiate(casingPrefab);
        Destroy(casing, 60);
        casing.transform.SetPositionAndRotation(casingFrom.position, casingFrom.rotation);
        casing.GetComponent<Rigidbody>().AddForce(startForce * casingFrom.up + Random.rotation  * (0.5f * Vector3.one), ForceMode.Impulse);
    }
}
