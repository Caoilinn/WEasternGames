using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : SoundSystem
{
    public CharacterSoundsLibrary characterSoundsLibrary;
    public GameObject head;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = head.AddComponent<AudioSource>();
        audioSource.minDistance = 3.5f;
        audioSource.dopplerLevel = 0;
        audioSource.spatialBlend = 1.0f; // set 3D
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAnimation_isGetCriticalHit() {
        // play hurt sounds
        AudioClip randomClip = RandomClip(characterSoundsLibrary.hurtSounds);
        PlayNewCharacterSound(randomClip);
    }
    
    private void PlayNewCharacterSound(AudioClip clip) {
        // immediately change clip regardless
        this.audioSource.clip = clip;

        //immediately play the voice
        this.audioSource.Play();
    }
}
