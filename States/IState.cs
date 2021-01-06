using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter(Enemy enemy);
    void Execute();
    void Exit();
}
