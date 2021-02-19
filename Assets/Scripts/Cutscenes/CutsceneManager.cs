using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject player;
    public GameObject playerCamera;
    public GameObject cutsceneCamera;

    public GameObject cutsceneLocation;
    private bool isAllDead = false;
    private PlayableDirector playableDirector;

    void Start() {
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.played += OnCutscenePlayed;
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemies.Length; i++) {
            if (enemies[i].activeSelf) {
                return;
            }
        }

        // else if there are no more enemies
        PlayCutscene();
    }

    private void PlayCutscene() {
        //Debug.Log("Cutscene is Played");
        if (playableDirector.state != PlayState.Playing) {
            playableDirector.Play();
        }
    }

    void OnCutscenePlayed(PlayableDirector director) {
        // move all characters in the cutscene to the cutscene location, reset local rotation to 0,0,0.
        player.transform.parent = cutsceneLocation.transform;
        
        player.transform.localPosition = new Vector3(0,0,0);
        player.transform.localEulerAngles = new Vector3(0,0,0);

        // deactivate the player
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerAnimation>().enabled = false;
        player.GetComponent<Rigidbody>().isKinematic = true;

        // deactivate the player camera
        playerCamera.SetActive(false);

        // activate the audio Listener in the cutscene camera
        cutsceneCamera.GetComponent<AudioListener>().enabled = true;
    }
}
