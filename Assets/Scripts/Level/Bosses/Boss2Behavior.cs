using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Behavior : StateMachineBehaviour
{
    Rigidbody2D _rb;
    Boss2Mechanics boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss2Mechanics>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PauseUIManager.GamePauseMenu)
            return;

        boss.Jump(boss.movingRight);
        if (boss.CheckGrounded())
            boss.Attack();

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
