using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacerEnemyAnimationActions : MonoBehaviour
{
    [SerializeField] PacerEnemy enemy;

    public void TurnAround(){
        enemy.TurnAround();
    }
}
