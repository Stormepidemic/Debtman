using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    
    public void clicked(){
        SceneManager.LoadScene("Demo_0",  LoadSceneMode.Single);
    }
}
