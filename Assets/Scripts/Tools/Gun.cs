using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : ITool
{
    Animator animator { get => ITool.animator; }
    ToolHandler handler { get => ITool.handler; }
    public int AttackCount = 0;
    public float AttackDamage = 50;
    public int activeAttack = 0;
    public int finishedAttack = 1;
    readonly int MaxAmmo = 8;
    public bool canAim = true;
    public bool isAiming = false;
    bool locked = false;
    public int TotalAmmo = 32;
    bool reloading = false;
    public bool AmmoHud = true;

    int ITool.AttackCount { get => AttackCount; set => AttackCount = value; }
    float ITool.AttackDamage { get => AttackDamage; set => AttackDamage = value; }
    int ITool.activeAttack { get => activeAttack; set => activeAttack = value; }
    int ITool.finishedAttack { get => finishedAttack; set => finishedAttack = value; }
    bool ITool.canAim { get => canAim; set => canAim = value; }
    bool ITool.isAiming { get => isAiming; set => isAiming = value; }
    bool ITool.AmmoHud { get => AmmoHud; }

    public Gun() {
        AttackCount = MaxAmmo;
        canAim = true;
    }
    public Gun(int MaxAmmo)
    {
        this.MaxAmmo = MaxAmmo;
        AttackCount = MaxAmmo;
        canAim = true;
    }

    public void Attack()
    {
        if(AttackCount > 0 && finishedAttack == 1 && !reloading)
        {
            AttackCount--;
            handler.AmmoText.text = AttackCount.ToString();
            finishedAttack = 0;
            animator.SetInteger("Attack", 1);
            //Debug.Log("Attacked " + AttackCount);
            if(AttackCount == 0)
            {
                animator.SetBool("Locked", true);
                animator.SetTrigger("OutOfAmmo");
            }
            /*else if(Random.Range(0,2) == 1)
            {
                animator.SetBool("Locked", true);
                locked = true;
            }/**/
        }
    }

    public void Reload()
    {
        if (TotalAmmo == 0 || reloading) return;
        reloading = true;
        animator.SetBool("Locked", false);
        animator.SetTrigger("Reloading");
        if (locked)
        {
            locked = false;
        }
        else
        {
            if (TotalAmmo < MaxAmmo)
            {
                AttackCount = TotalAmmo;
            }
            else
            {
                AttackCount = (AttackCount > 0 ? MaxAmmo+1:MaxAmmo);
                TotalAmmo -= MaxAmmo;
            }
            handler.TotalAmmoText.text = "/"+TotalAmmo;
            handler.AmmoText.text = AttackCount.ToString();
        }
    }

    public void EndReload()
    {
        reloading = false;
    }

    public void Aim()
    {
        if (!isAiming)
        {
            isAiming = true;
            animator.SetBool("Aiming", true);
        }
    }
    public void EndAim()
    {
        if (isAiming)
        {
            isAiming = false;
            animator.SetBool("Aiming", false);
        }
    }

    public void ResetTool()
    {
        animator.SetInteger("Attack", 0);
        finishedAttack = 1;
    }

    public void AddAmmo(int amount)
    {
        TotalAmmo += amount;
        handler.TotalAmmoText.text = "/" + TotalAmmo;
    }
}
