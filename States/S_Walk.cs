using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Walk : IState
{
    private Enemy enemy;

    private float
        walkTimePaused,
        walkTimer;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        walkTimePaused = Random.Range(3f, 6f);
        //this.enemy.SetAnimation("walking");
        this.enemy.Move();
    }

    public void Execute()
    {
        Walking();
        Debug.Log("walking");
    }

    public void Exit()
    {
    }

    private void Walking()
    {
        if (enemy.target != null)
        {
            enemy.ChangeState(new S_Follow());
        }

        else if(walkTimer <= walkTimePaused)
        {
            walkTimer += Time.deltaTime;
        }
        else
        {
            walkTimer = 0f;
            enemy.ChangeState(new S_Idle());
        }
    }
}
