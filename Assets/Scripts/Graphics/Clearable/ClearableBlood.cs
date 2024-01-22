using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearableBlood : ClearableElement
{
    [SerializeField] private float spreadRate; //How quickly the blood spreads from it's base size.
    [SerializeField] private float maxSpreadSize; //How big should this blood get before it stops spreading?
    private Vector3 baseScale;
    private Vector3 maxScale;

    private LayerMask mask;
    private RaycastHit hit;

    void Start(){
        this.clearType = ClearableCondition.PLAYER_DEATH;
        baseScale = gameObject.transform.localScale;
        maxScale = baseScale*maxSpreadSize;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, maxScale, spreadRate*Time.deltaTime);
        //Make sure this blood snaps to the ground
        // if(Physics.Raycast(gameObject.transform.position, Vector3.up, out hit, Mathf.Infinity, mask)){
        //     gameObject.transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
        // }else{
        //     //gameObject.transform.position = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
        // }
    }

    public override void ClearElement(){
        Destroy(gameObject);
    }

    public override void ClearElement(ClearableCondition condition){
        if(condition == this.clearType){
            Destroy(gameObject);
        }
    }


}
