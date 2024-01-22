using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject landingCrackPreFab;

    private void DoLandingEffect(){
        Instantiate(landingCrackPreFab, gameObject.transform.parent.gameObject.transform.position, Quaternion.identity);
    }

    private void SetCanMove(){
        GameObject parent = gameObject.transform.parent.gameObject;
        PlayerMovement player = parent.GetComponent<PlayerMovement>();
        player.HandlePlayerSpawn();
    }
}
