using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : StateMachineBehaviour
{
    Rigidbody2D _rb;
    Boss1Mechanics boss;
    public float _speed = 3.5f;
    public float _attackRange = 3f;
    bool inAttackRange;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss1Mechanics>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PauseUIManager.GamePauseMenu)
            return;

        boss.LookAtPlayer();
        if (!inAttackRange)
        {
            Vector2 target = new Vector2(Player.Instance.transform.position.x, _rb.position.y);
            _rb.position = Vector2.MoveTowards(_rb.position, target, _speed * Time.deltaTime);
        }

        if (Vector2.Distance(_rb.position, Player.Instance.transform.position) <= _attackRange)
        {
            inAttackRange = true;
            animator.SetTrigger("Attack");
        }
        else inAttackRange = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
