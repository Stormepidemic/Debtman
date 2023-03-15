using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNode : MonoBehaviour
{
    private float distanceToPlayer;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, gameObject.transform.position);
    }

    public float getPlayerDistance(){
        return distanceToPlayer;
    }
}
