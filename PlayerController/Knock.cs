using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockDelegate
{
    //private Enemy enemyToKnock;
    //private GameObject isKnockedBy;

    public delegate void KnockEnemy(bool knockToRight, Vector2 force);
    public static event KnockEnemy knockEvent;

    //public KnockD(Enemy enemyToKnock, GameObject isKnockedBy)
    //{
    //    this.enemyToKnock = enemyToKnock;
    //    this.isKnockedBy = isKnockedBy;
    //}

    public static void Knocking(bool knockToRight, Vector2 force)
    {
        knockEvent?.Invoke(knockToRight, force);
    }
}
