using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public float MaxHealth = 100;
    public float health = 100;
    public float defense = 10;
    public float destroyAfter = 5;

    public UnityEvent deathEvent;

    EnemyController controller;

    private void Start()
    {
        controller = GetComponent<EnemyController>();
    }

    public void Heal(float health)
    {
        this.health += health;
        if (this.health > MaxHealth) this.health = MaxHealth;
    }

    public void Damage(float attack)
    {
        if (attack == 0) return;
        health -= CalcDamage(attack);
        if (health <= 0) {
            controller.alive = false;
            deathEvent.Invoke(); 
            if (destroyAfter > 0) Destroy(gameObject, destroyAfter);
            enabled = false;
        }
    }

    float CalcDamage(float attack)
    {
        float f = Mathf.Pow(attack / defense, 2);
        float h = attack * ((f <= 1) ? f : 1);
        h -= defense / attack;
        return h;
    }

    public void Kill()
    {
        deathEvent.Invoke();
        enabled = false;
        controller.alive = false;
        if (destroyAfter > 0) Destroy(gameObject, destroyAfter);
    }
}