using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StartGame : MonoBehaviour
{
    
    public void clicked(){
        SceneManager.LoadScene("Hub");
        SceneManager.UnloadSceneAsync("Hub");
    }
}
