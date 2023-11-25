using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public interface ITool
{
    public static Animator animator;
    public static ToolHandler handler;
    public int AttackCount { get; set; }
    public float AttackDamage { get; set; }
    public int activeAttack { get; set; }
    public int finishedAttack { get; set; }
    public bool canAim { get; set; }
    public bool isAiming { get; set; }
    public bool AmmoHud { get; }

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
        handler.usingTool = false;
    }

    public virtual void AddAmmo(int v) { }

    public virtual void EndReload() { }
}
