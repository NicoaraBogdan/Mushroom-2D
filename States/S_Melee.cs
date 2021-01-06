using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Melee : IState
{
    private Enemy enemy;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        this.enemy.Stop();
    }

    public void Execute()
    {
        Debug.Log("mele state");

        if (enemy.canAttackMele)
        {
            Debug.Log("attack");
            enemy.ResetTimers("mele");
        }
        else if (!enemy.InMeleRange)
        {
            if (enemy.InThrowRange && enemy.canAttackThrow)
            {
                enemy.ChangeState(new S_Range());
            }

            if (enemy.target == null)
                enemy.ChangeState(new S_Idle());

            else enemy.ChangeState(new S_Follow());
        }
    }
             
    public void Exit()
    {
        enemy.Move();
    }
}
