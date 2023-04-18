using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KIllEnemyAction : MonoBehaviour
{
    [SerializeField] private GameObject hurtbox;

    void Kill(){
        //hurtbox.GetComponent<Enemy>().Disable();
        //gameObject.GetComponent<Enemy>().Disable();
        Destroy(gameObject.transform.parent.gameObject);
    }
}
