using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonRTS_demo_Knight_Animations: UnitAnimationHandler
{
    private Animator anim;
    public float AttackRechargeTime = 0.9f;
    private bool doAttack = false;
    private bool doNotWaitForAttack = true;
    private bool doMove = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (doMove)
        {
            ChargeAnimation();
        } else
        {
            IdleAnimation();
        }

        if (doAttack && doNotWaitForAttack)
        {
            StartCoroutine(AttackAnimation());
        }
    }

    public override void Attack()
    {
        doAttack = true;
    }

    public override void IsMoving(bool movingStatus)
    {
        doMove = movingStatus;
    }

    #region Animations

    private void IdleAnimation()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void WalkAnimation()
    {
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void ChargeAnimation()
    {
        anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    private IEnumerator AttackAnimation()
    {
        doNotWaitForAttack = false;
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 1);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(AttackRechargeTime);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack Layer"), 0);
        doAttack = false;
        doNotWaitForAttack = true;
    }

    #endregion

}
