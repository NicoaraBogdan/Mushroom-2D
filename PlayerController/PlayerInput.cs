using UnityEngine;
using System.Collections;

public class PlayerInput: MonoBehaviour
{
    public static PlayerInput Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void HandleInputKeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) PlayerCombat.Instance.AttemptToDash();


        if (Input.GetKeyDown(KeyCode.Space)) PlayerCombat.Instance.Jump();


        if (Input.GetKeyUp(KeyCode.Space)) PlayerCombat.Instance.StopJump();


        if (Input.GetKeyDown(KeyCode.S)) PlayerCombat.Instance.StartMarioAttack();


        if (Input.GetKeyDown(KeyCode.Q)) PlayerCombat.Instance.Attack();
    }
}
