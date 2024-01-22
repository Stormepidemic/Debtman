using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Shadow : MonoBehaviour
{
    public GameObject obj; //The player GameObject
    private RaycastHit hit;
    public float verticalOffset;
    //public int layerMask = 1 << 6;
    LayerMask mask;
    [SerializeField] private float currentScale; //The current shadow size

    void Start(){
        mask = LayerMask.GetMask("Ground");
    }
    void FixedUpdate()
    {
        if(obj == null || !obj.activeInHierarchy){
            Destroy(gameObject);
        }else{
            if(Physics.Raycast(obj.transform.position, -Vector3.up, out hit, Mathf.Infinity, mask)){
                gameObject.transform.position = new Vector3(hit.point.x, hit.point.y + verticalOffset, hit.point.z);
            }else{
                gameObject.transform.position = new Vector3(1000.0f, 1000.0f, 1000.0f); //Sends the shadow away, essentially making it disappear from view
            }
        }
        
    }
}
