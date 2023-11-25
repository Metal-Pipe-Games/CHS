using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tool
{
    public static Animator animator;
    public int AttackCount = 1;
    public float AttackDamage = 0;
    public int activeAttack = 0;
    public int finishedAttack = 0;
    public bool canAim = false;
    public bool isAiming = false;

    public virtual void Attack()
    {
        if (finishedAttack == activeAttack)
        {
            activeAttack++;
            animator.SetInteger("Attack", activeAttack);
        }
    }
    public virtual void Aim()
    {
        if (!isAiming)
        {
            isAiming = true;
            animator.SetBool("Aiming", true);
        }
    }
    public virtual void EndAim()
    {
        if (isAiming)
        {
            isAiming = false;
            animator.SetBool("Aiming", false);
        }
    }
    public virtual void Reload() { }
    public virtual void ResetTool()
    {
        activeAttack = finishedAttack = 0;
        animator.SetBool("Timeout", false);
        animator.SetInteger("Attack", 0);
    }
}
