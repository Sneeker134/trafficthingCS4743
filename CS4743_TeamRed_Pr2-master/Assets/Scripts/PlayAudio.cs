using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    //creates a place holder for the audio
    public AudioSource CarHorn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Plays the AudioSource when button is pressed.
    void playCarHorn()
    {
        //Plays CarHorn.
        CarHorn.Play();
    }
}
