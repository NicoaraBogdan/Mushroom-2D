using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Range : IState
{
    private Enemy enemy;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        this.enemy.Stop();
    }

    public void Execute()
    {
        Debug.Log("throw");
        
        if(enemy.target != null)
        {
            enemy.ChangeState(new S_Follow());
        }
        else
        {
            enemy.ChangeState(new S_Idle());
        }
        
    }

    public void Exit()
    {
        enemy.Move();
    }
}
