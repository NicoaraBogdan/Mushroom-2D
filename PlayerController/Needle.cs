using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    [SerializeField]
    private Vector2 knockForce;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("Ground")) && 
            PlayerCombat.Instance.MarioAttack)
        {

            PlayerController.Instance.AddBounceMA();
            PlayerCombat.Instance.MarioAttack = false;
            _animator.SetTrigger("MALand");
        }

        else if (other.CompareTag("Enemy") && PlayerCombat.Instance.GetAttackState() == 3)
        {
            KnockDelegate.Knocking(PlayerCombat.Instance.EnemyToTheLeft(other.transform), knockForce);
        }
    }
}
