using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Spawning : IState
{
    private Enemy enemy;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        this.enemy.isSpawing = true;
    }

    public void Execute()
    {
        if (enemy.isSpawing)
        {
            enemy.Climb();
        }
        else enemy.ChangeState(new S_Idle());
    }

    public void Exit()
    {

    }

}
