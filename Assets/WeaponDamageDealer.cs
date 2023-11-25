using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponDamageDealer : MonoBehaviour
{
    public float Attack;
    ITool weapon;
    [InspectorName("Weapon")]
    public Tools weaponType;
    public ToolHandler handler;
    public VisualEffect vfx;
    // Start is called before the first frame update
    void Start()
    {
        weapon = handler.tools[weaponType];
        Attack = weapon.AttackDamage;
    }

    // Update is called once per frame
    Vector3 enteringPosition = Vector3.zero;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.transform.root.GetComponent<EnemyHealth>().Damage(Attack);
            enteringPosition = transform.position;
        }
        /*else if (other.CompareTag("Level"))
        {

        }/**/
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!transform.position.Equals(enteringPosition))
            {
                Vector3 direction = transform.position - enteringPosition;
                vfx.gameObject.transform.up = direction;
                vfx.transform.position = other.ClosestPoint(transform.position);
                vfx.SendEvent("OnEnemyMinorHit");
                enteringPosition = transform.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Vector3 direction = transform.position - enteringPosition;
            vfx.gameObject.transform.up = direction;
            vfx.transform.position = other.ClosestPoint(transform.position);
            vfx.SendEvent("OnEnemyHit");
        }
    }
}
