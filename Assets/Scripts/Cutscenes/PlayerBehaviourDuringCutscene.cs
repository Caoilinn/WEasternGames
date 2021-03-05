using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerBehaviourDuringCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    // Start is called before the first frame update
    void Start()
    {
        playableDirector.played += OnCutscenePlayed;
    }

    void OnCutscenePlayed(PlayableDirector director) {
        this.GetComponent<PlayerMovement>().enabled = false;
        this.GetComponent<PlayerAnimation>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }
}
