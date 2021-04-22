using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalPunchAnimScript : StateMachineBehaviour
{
    bool isDamageInit = false;
    float damageStart = 0.0f;
    SkillDataBase skillDataBase;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        damageStart = 0.5f;
        if (damageStart>=stateInfo.normalizedTime && !isDamageInit)
        {
            isDamageInit = true;
            skillDataBase = animator.gameObject.GetComponentInParent<PlayerControl>().curCastingSkill;
            skillDataBase.Damage();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponentInParent<PlayerControl>().ChangeState(PlayerControl.STATE.IDLE);
        isDamageInit = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
