using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerSceneSwap : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private GameObject music;
    [SerializeField] private AudioClip otherSceneMusic;
    // Start is called before the first frame update
    void Start()
    {
        music = GameObject.Find("Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(){
        music.GetComponent<AudioSource>().clip = otherSceneMusic;
        SceneManager.LoadScene(sceneName,  LoadSceneMode.Single);
    }
}
