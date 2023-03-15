using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartGame : MonoBehaviour
{
    
    public void clicked(){
        SceneManager.LoadScene("Demo_0");
        SceneManager.UnloadSceneAsync("Demo_0");
        print("START");
    }
}
