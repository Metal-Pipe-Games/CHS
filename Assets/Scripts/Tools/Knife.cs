using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : ITool
{
    Animator animator { get => ITool.animator; }
    public int AttackCount = 2;
    public float AttackDamage = 25;
    public int activeAttack = 0;
    public int finishedAttack = 0;
    public bool canAim = false;
    public bool isAiming = false;
    public bool Attacking = false;
    public bool AmmoHud = false;

    int ITool.AttackCount { get => AttackCount; set => AttackCount = value; }
    float ITool.AttackDamage { get => AttackDamage; set => AttackDamage = value; }
    int ITool.activeAttack { get => activeAttack; set => activeAttack = value; }
    int ITool.finishedAttack { get => finishedAttack; set => finishedAttack = value; }
    bool ITool.canAim { get => canAim; set => canAim = value; }
    bool ITool.isAiming { get => isAiming; set => isAiming = value; }
    bool ITool.AmmoHud { get => AmmoHud; }

    public void Attack()
    {
        if (finishedAttack == activeAttack)
        {
            activeAttack++;
            Attacking = true;
            ITool.animator.SetInteger("Attack", activeAttack);
        }
    }
    public void Aim() { }
    public void EndAim() { }

    public virtual void ResetTool()
    {
        activeAttack = finishedAttack = 0;
        Attacking = false;
        animator.SetBool("Timeout", false);
        animator.SetInteger("Attack", 0);
    }
}
