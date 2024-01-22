using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFog : MonoBehaviour
{
    private Vector3 currentPosition;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null){
            player = GameObject.Find("Player_Character");
        }else{
            if(transform.position != currentPosition){
                transform.position = player.transform.position;
            }else{
                currentPosition = transform.position;
            }
        }
    }
}
