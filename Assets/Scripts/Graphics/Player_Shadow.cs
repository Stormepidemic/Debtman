using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shadow : MonoBehaviour
{
    public GameObject obj; //The player GameObject
    private RaycastHit hit;
    public float verticalOffset;
    public int layerMask = 1 << 8;
    [SerializeField] private float currentScale; //The current shadow size

    LayerMask mask;

    void Start(){
        mask = LayerMask.GetMask("Ground");
    }
    // Update is called once per frame
    void Update()
    {
        if(obj == null){
            Destroy(this);
        }
        if(Physics.Raycast(obj.transform.position, -Vector3.up, out hit, Mathf.Infinity, mask)){

            gameObject.transform.position = new Vector3(hit.point.x, hit.point.y + verticalOffset, hit.point.z);
        }else{
            gameObject.transform.position = new Vector3(1000.0f, 1000.0f, 1000.0f); //Sends the shadow away, essentially making it disappear from view
        }
    }
}
