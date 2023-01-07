using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    GameObject camera;
    private PostProcessVolume PPVolumeTransition;
    private ChromaticAberration chromAb;



    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera");
        PPVolumeTransition=camera.GetComponent<PostProcessVolume>();
        PPVolumeTransition.profile.TryGetSettings(out chromAb);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginCoolTransitionEffect(){
        chromAb.intensity.value=chromAb.intensity.value+(float)1;
    }
    
    public void ChangeSceneToMultiplayer(){
        BeginCoolTransitionEffect();
        SceneManager.LoadScene("SampleScene");
    }
    public void ChangeSceneToSingleplayer(){
        SceneManager.LoadScene("SampleScene");
    }
    public void ChangeSceneToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    
}
