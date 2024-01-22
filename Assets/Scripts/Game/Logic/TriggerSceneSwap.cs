using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriggerSceneSwap : Volume
{
    [SerializeField] private string sceneName;
    private GameObject music;
    [SerializeField] private AudioClip otherSceneMusic;
    [SerializeField] private GameObject facePoint;
    // Start is called before the first frame update
    void Start()
    {
        music = GameObject.Find("Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    IEnumerator SceneChangeFade(){
        GameObject fade = GameObject.Find("SceneSwap_Fade");
        Image graphic = fade.GetComponent<Image>();
        yield return new WaitForSeconds(3);
        // fade from transparent to opaque
        // loop over 2 second
            for (float i = 0; i <= 2; i += Time.deltaTime){
            // set color with i as alpha
            graphic.color = new Color(100, 0, 0, i);
            yield return null;
        }

        music.GetComponent<AudioSource>().clip = otherSceneMusic;
        SceneManager.LoadScene(sceneName,  LoadSceneMode.Single); //Load the next scene!
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            other.gameObject.GetComponent<PlayerMovement>().SetCanMove(false);
            if(facePoint != null){
                other.gameObject.GetComponent<PlayerMovement>().HandleExitLevel(facePoint);
            }else{
                other.gameObject.GetComponent<PlayerMovement>().HandleExitLevel();
            }
            
        }
        StartCoroutine(SceneChangeFade());
        
    }
}
