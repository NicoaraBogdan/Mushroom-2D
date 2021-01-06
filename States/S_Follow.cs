using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Follow : IState
{
    private Enemy enemy;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        this.enemy.Move();
    }

    public void Execute()
    {
        Follow();
        Debug.Log("following");
    }

    public void Exit()
    {
    }

    private void Follow()
    {
        if(enemy.target != null)
        {
            enemy.LookAtPlayer();

            if (enemy.InMeleRange) enemy.ChangeState(new S_Melee());

            else if (enemy.InThrowRange && enemy.canAttackThrow) enemy.ChangeState(new S_Range());
        }
        else
        {
            enemy.ChangeState(new S_Idle());
        }
    }
}
