using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool playSound = false, endSound = false;
    bool checkBool = false;
    AudioSource _Audio;

    void Start()
    {
        _Audio = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (playSound && !checkBool)
        {
            checkBool = true;
            _Audio.Play();
        }
        if (endSound && playSound)
        {
            _Audio.volume -= Time.deltaTime * 2;
            if (_Audio.volume <= 0.05f)
            {
                _Audio.Stop();
                _Audio.volume = 1f;
                endSound = false;           
            }
        
        }
    }
}
