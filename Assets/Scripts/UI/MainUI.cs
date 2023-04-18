using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] SlidingUI inventory;
    [SerializeField] GameObject contract;
    [SerializeField] SlidingUI counters; //Scrap and Lives counter
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleShowUIElements();
    }

    //Handles showing the UI from the MainUI which holds the collected items as well as the extra lives and scrap
    private void HandleShowUIElements(){
        if(Input.GetButtonDown("Show")){
            counters.Show();
            inventory.Show();
        }
    }

    public void ShowCounters(){
        counters.Show();
    }

    public void ShowInventory(){
        inventory.Show();
    }

    public void ShowContract(){
        contract.GetComponent<SpriteAnimator>().enabled = true;
    }
}
