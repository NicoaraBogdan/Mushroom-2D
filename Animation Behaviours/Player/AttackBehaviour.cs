using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerController.Instance.StopMovment();
        PlayerCombat.Instance.Attacking = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (PlayerCombat.Instance.GetAttackState())
        {
            case 2:
                if (PlayerCombat.Instance.AttackChain)
                {
                    PlayerController.Instance.SetAnimation("attack");
                    PlayerCombat.Instance.SetAttackState(3);
                }
                else
                {
                    PlayerController.Instance.CanMove();
                    PlayerCombat.Instance.Attacking = false;
                    PlayerCombat.Instance.SetAttackState(1);
                }
                break;

            case 3:
                PlayerController.Instance.CanMove();
                PlayerCombat.Instance.Attacking = false;
                PlayerCombat.Instance.AttackChain = false;
                PlayerCombat.Instance.SetAttackState(1);
                break;
        }
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
