using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class S_Idle : IState
{
    private Enemy enemy;

    private float
        idleTimePaused,
        idleTimer = 0;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        idleTimePaused = Random.Range(3f, 6f);
        //this.enemy.SetAnimation("idle");
        this.enemy.Stop();
    }

    public void Execute()
    {
        Debug.Log("idleing");
        Idle();
    }

    public void Exit()
    {

    }

    private void Idle()
    {

        if (enemy.target != null)
        {
            enemy.ChangeState(new S_Follow());
        }

        else if (idleTimer <= idleTimePaused)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0f;
            enemy.ChangeState(new S_Walk());
        }

    }
}
