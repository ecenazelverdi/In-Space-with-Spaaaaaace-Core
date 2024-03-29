using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 0f;
    [SerializeField] AudioSource playerVoice;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticels;

    bool collisionStatues = true;

    private void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
            LoadNextLevel();
        if (Input.GetKey(KeyCode.C))
        {
            collisionStatues = !collisionStatues;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Oh, a friend ^^");
                break;
            case "Finish":
                Debug.Log("Yay, finished!");
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("Yummy :p");
                break;
            default:
                Debug.Log("Oopsy Daisy :P");
                if(collisionStatues)
                    StartCrashSequence();
                break;
        }
        
    }

    void StartCrashSequence(){
        GetComponent<Movement>().isAlive = false;
        GetComponent<Movement>().enabled = false;
        crashParticels.Play();
        levelLoadDelay = GetComponent<AudioForPlayer>().clipLength;
        if(levelLoadDelay <= 0)
        {
            Invoke("StartCrashSequence", 0f);
            levelLoadDelay = GetComponent<AudioForPlayer>().clipLength;
        }
        else
            Invoke("ReloadScene", levelLoadDelay);
    }
    
    void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            
        SceneManager.LoadScene(currentSceneIndex);
        GetComponent<Movement>().enabled = true;

    }
    void StartSuccessSequence(){
        GetComponent<Movement>().success = true;
        GetComponent<Movement>().enabled = false;
        successParticles.Play();
        levelLoadDelay = GetComponent<AudioForPlayer>().clipLength;
        if (levelLoadDelay <= 0)
        {
            Invoke("StartSuccessSequence", 0f);
            levelLoadDelay = GetComponent<AudioForPlayer>().clipLength;
        }
        else
            Invoke("LoadNextLevel", levelLoadDelay);

    }
    void LoadNextLevel()
    {
        GetComponent<Movement>().success = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)//SceneManager.sceneCountInBuildSettings -> Total number of scene
            nextSceneIndex = 0;
        GetComponent<AudioForPlayer>().count = 0;
        SceneManager.LoadScene(nextSceneIndex);       
    }

}
